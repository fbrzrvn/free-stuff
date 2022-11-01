import supertest, { SuperTest, Test } from 'supertest';
import { RefreshTokenDto } from '../../../src/api/auth/dto';

import { App } from '../../../src/App';
import { Db } from '../../config/Db';
import { getLoginDto, getRegisterDto } from '../../fixtures/user';

describe('Auth test suite', () => {
  const app = new App();
  let server: SuperTest<Test>;

  let access_token: string;
  let refresh_token: string;

  beforeAll(async () => {
    await Db.connect();
    server = supertest(app.getHTTPServer());
  });

  afterAll(async () => {
    await Db.close();
    await app.stop();
  });

  describe('POST /api/auth/register', () => {
    it('should return 400 as status code when username fail validation (too short)', async () => {
      // Arrange
      const userData = getRegisterDto('ler');

      // Act
      const response = await server.post('/api/auth/register').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('username must be longer than or equal to 4 characters');
    });

    it('should return 400 as status code when username fail validation (too long)', async () => {
      // Arrange
      const userData = getRegisterDto('lerelleDeBarcelona');

      // Act
      const response = await server.post('/api/auth/register').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('username must be shorter than or equal to 15 characters');
    });

    it('should return 400 as status code when email fail validation', async () => {
      // Arrange
      const userData = getRegisterDto(undefined, 'lerelleemail.com');

      // Act
      const response = await server.post('/api/auth/register').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('email must be an email');
    });

    it('should return 400 as status code when password fail validation', async () => {
      // Arrange
      const userData = getRegisterDto(undefined, undefined, 'lere');

      // Act
      const response = await server.post('/api/auth/register').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('password must be longer than or equal to 6 characters');
    });

    it('should return 201 as status code with the user', async () => {
      // Arrange
      const userData = getRegisterDto();

      // Act
      const response = await server.post('/api/auth/register').send(userData).expect(201);

      // Assert
      expect(response.status).toBe(201);
      expect(response.body).toBeDefined();
    });

    it('should return 400 as status code when user already exist', async () => {
      // Arrange
      const userData = getRegisterDto();

      // Act
      const response = await server.post('/api/auth/register').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('User already exist');
    });
  });

  describe('POST /api/auth/login', () => {
    it('should return 400 as status code when email is invalid', async () => {
      // Arrange
      const userData = getLoginDto('lerelle12@email.com');

      // Act
      const response = await server.post('/api/auth/login').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('Invalid credentials');
    });

    it('should return 400 as status code when password is invalid', async () => {
      // Arrange
      const userData = getLoginDto(undefined, 'lerelle12');

      // Act
      const response = await server.post('/api/auth/login').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('Invalid credentials');
    });

    it('should return 400 as status code when email and password are invalid', async () => {
      // Arrange
      const userData = getLoginDto('lerelle12@email.com', 'lerelle12');

      // Act
      const response = await server.post('/api/auth/login').send(userData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('Invalid credentials');
    });

    it('should return 200 as status code with a token', async () => {
      // Arrange
      const userData = getLoginDto();

      // Act
      const response = await server.post('/api/auth/login').send(userData).expect(200);

      access_token = response.body.accessToken;
      refresh_token = response.body.refreshToken;

      // Assert
      expect(response.status).toBe(200);
      expect(response.body).toBeDefined();
    });
  });

  describe('POST /api/auth/refresh-token', () => {
    it('should return 400 as status code when the token is not a jwt token', async () => {
      // Arrange
      const tokenData: RefreshTokenDto = { token: 'fak3.t0k3n' };

      // Act
      const response = await server.post('/api/auth/refresh-token').send(tokenData).expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('jwt malformed');
    });

    it('should return 403 as status code when the token is not valid', async () => {
      // Arrange
      const tokenData: RefreshTokenDto = {
        token:
          'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjYzNjEwYTRiODYyZTQ4NjZlOTk0MDkxNSIsImlhdCI6MTY2NzMwNDAxMiwiZXhwIjoxNjY3MzA0OTEyLCJ0eXBlIjoiYWNjZXNzIn0.pBdPiKfvc_Zd8pU7bDbtV44QqduBopEM9ILSFeag9yA'
      };

      // Act
      const response = await server.post('/api/auth/refresh-token').send(tokenData).expect(403);

      // Assert
      expect(response.status).toBe(403);
      expect(response.body.message).toBe('Invalid token');
    });

    it('should return 200 as status code with the new generated tokens', async () => {
      // Arrange
      const tokenData: RefreshTokenDto = { token: refresh_token };

      // Act
      const response = await server.post('/api/auth/refresh-token').send(tokenData).expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body).toBeDefined();
    });
  });

  describe('POST /api/auth/logout', () => {
    it('should return 401 as status code when user in logged in', async () => {
      // Act
      const response = await server.post('/api/auth/logout').expect(401);

      // Assert
      expect(response.status).toBe(401);
    });

    it('should return 200 as status code', async () => {
      // Act
      const response = await server
        .post('/api/auth/logout')
        .set('Authorization', 'Bearer ' + access_token)
        // .set('Cookie', [`access_token=${access_token}`])
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body.message).toBe('Logout success');
    });
  });
});
