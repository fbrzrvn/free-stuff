import supertest, { SuperTest, Test } from 'supertest';

import { CreateUserDto, UpdateUserDto } from '../../../src/api/users/dto';
import { App } from '../../../src/App';
import { Db } from '../../config/Db';
import { mappedUser } from '../../fixtures/user';
import { getUserClaims } from '../../helpers';

describe('Users test suite', () => {
  const app = new App();
  let server: SuperTest<Test>;

  let _id: string;
  let _token: string;

  beforeAll(async () => {
    await Db.connect();
    server = supertest(app.getHTTPServer());

    const { id, token } = await getUserClaims(server);

    _id = id;
    _token = token;
  });

  afterAll(async () => {
    await Db.close();
    await app.stop();
  });

  describe('GET /api/users/profile', () => {
    it('should return 401 as status code when user is not authenticated', async () => {
      // Act
      const response = await server.get('/api/users/profile').expect(401);

      // Assert
      expect(response.status).toBe(401);
    });

    it('should return 200 as status code with the current user', async () => {
      // Arrange
      const user = { id: _id, ...mappedUser };

      // Act
      const response = await server
        .get('/api/users/profile')
        .set('Authorization', 'Bearer ' + _token)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body).toMatchObject(user);
    });
  });

  describe('PUT /api/users/profile/update', () => {
    it('should return 401 as status code when user is not authenticated', async () => {
      // Act
      const response = await server.put('/api/users/profile/update').expect(401);

      // Assert
      expect(response.status).toBe(401);
    });

    it('should return 400 as status code if user when user data fail the validation', async () => {
      // Arrange
      const userData: UpdateUserDto = {
        country: ''
      };

      // Act
      const response = await server
        .put('/api/users/profile/update')
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('country should not be empty');
    });

    it('should return 200 as status code with the updated user', async () => {
      // Arrange
      const userData: UpdateUserDto = { country: 'IT' };

      const user = { id: _id, country: userData.country, ...mappedUser };

      // Act
      const response = await server
        .put('/api/users/profile/update')
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body).toMatchObject(user);
    });
  });

  describe('POST /api/users/create', () => {
    it('should return 400 as status code when user data fail the validation', async () => {
      // Arrange
      const userData: CreateUserDto = {
        username: 'man',
        email: 'manoloemail.com',
        password: 'manolo'
      };

      // Act
      const response = await server
        .post('/api/users/create')
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe(
        'username must be longer than or equal to 4 characters, email must be an email'
      );
    });

    it('should return 201 as status code', async () => {
      // Arrange
      const userData: CreateUserDto = {
        username: 'manolo',
        email: 'manolo@email.com',
        password: 'manolo'
      };

      // Act
      const response = await server
        .post('/api/users/create')
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(201);

      // Assert
      expect(response.status).toBe(201);
      expect(response.body).toBeDefined();
    });
  });

  describe('GET /api/users', () => {
    it('should return 200 as status code', async () => {
      // Act
      const response = await server
        .get('/api/users')
        .set('Authorization', 'Bearer ' + _token)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body.meta.totalDocs).toBe(2);
    });

    it('should return 200 as status code and apply the query params', async () => {
      // Act
      const response = await server
        .get('/api/users?limit=1')
        .set('Authorization', 'Bearer ' + _token)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body.data.length).toBe(1);
    });
  });

  describe('GET /api/users/:id', () => {
    it('should return 400 as status code when id is not a valid mongoId', async () => {
      // Act
      const response = await server
        .get(`/api/users/63503d596`)
        .set('Authorization', 'Bearer ' + _token)
        .expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('id must be a mongodb id');
    });

    it('should return 404 as status code when user not exist with request id', async () => {
      // Act
      const response = await server
        .get(`/api/users/63503d5964dba85c9233c231`)
        .set('Authorization', 'Bearer ' + _token)
        .expect(404);

      // Assert
      expect(response.status).toBe(404);
      expect(response.body.message).toBe('User not found');
    });

    it('should return 200 as status code', async () => {
      // Arrange
      const user = { id: _id, ...mappedUser };

      // Act
      const response = await server
        .get(`/api/users/${_id}`)
        .set('Authorization', 'Bearer ' + _token)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body).toMatchObject(user);
    });
  });

  describe('PUT /api/users/:id/update', () => {
    it('should return 400 as status code when id is not a valid mongoId', async () => {
      // Arrange
      const userData: UpdateUserDto = { country: 'IT' };

      // Act
      const response = await server
        .put(`/api/users/63503d596/update`)
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('id must be a mongodb id');
    });

    it('should return 404 as status when user not exist with request id', async () => {
      // Arrange
      const userData: UpdateUserDto = { country: 'IT' };

      // Act
      const response = await server
        .put(`/api/users/63503d5964dba85c9233c231/update`)
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(404);

      // Assert
      expect(response.status).toBe(404);
      expect(response.body.message).toBe('User not found');
    });

    it('should return 400 as status code if user when user data fail the validation', async () => {
      // Arrange
      const userData: UpdateUserDto = {
        country: ''
      };

      // Act
      const response = await server
        .put(`/api/users/${_id}/update`)
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('country should not be empty');
    });

    it('should return 200 as status code', async () => {
      // Arrange
      const userData: UpdateUserDto = {
        country: 'IT'
      };

      const user = { id: _id, ...mappedUser, ...userData };

      // Act
      const response = await server
        .put(`/api/users/${_id}/update`)
        .set('Authorization', 'Bearer ' + _token)
        .send(userData)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
      expect(response.body).toMatchObject(user);
    });
  });

  describe('DELETE /api/users/:id/delete', () => {
    it('should return 400 as status code when id is not a valid mongoId', async () => {
      // Act
      const response = await server
        .delete(`/api/users/63503d596/delete`)
        .set('Authorization', 'Bearer ' + _token)
        .expect(400);

      // Assert
      expect(response.status).toBe(400);
      expect(response.body.message).toBe('id must be a mongodb id');
    });

    it('should return 404 as status when user not exist with request id', async () => {
      // Act
      const response = await server
        .delete(`/api/users/63503d5964dba85c9233c231/delete`)
        .set('Authorization', 'Bearer ' + _token)
        .expect(404);

      // Assert
      expect(response.status).toBe(404);
      expect(response.body.message).toBe('User not found');
    });

    it('should return 200 as status code', async () => {
      // Act
      const response = await server
        .delete(`/api/users/${_id}/delete`)
        .set('Authorization', 'Bearer ' + _token)
        .expect(200);

      // Assert
      expect(response.status).toBe(200);
    });
  });
});
