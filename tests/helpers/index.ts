import { SuperTest, Test } from 'supertest';

import { LoginDto, RegisterDto } from '../../src/api/auth/dto';

const getUserClaims = async (server: SuperTest<Test>) => {
  let id: string;
  let token: string;

  const newUser: RegisterDto = {
    username: 'lerelle',
    email: 'lerelle@email.com',
    password: 'lerelle'
  };

  const user: LoginDto = {
    email: 'lerelle@email.com',
    password: 'lerelle'
  };

  const userResponse = await server.post('/api/auth/register').send(newUser);

  const tokenResponse = await server.post('/api/auth/login').send(user);

  id = userResponse.body.id;
  token = tokenResponse.body.accessToken;

  return { id, token };
};

export { getUserClaims };
