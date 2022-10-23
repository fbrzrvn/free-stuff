import supertest, { SuperTest, Test } from 'supertest';

import { App } from '../src/App';

describe('App test suite', () => {
  const app = new App();
  let server: SuperTest<Test>;

  beforeAll(async () => {
    server = supertest(app.getHTTPServer());
  });

  afterAll(async () => {
    await app.stop();
  });

  describe('GET /api/status', () => {
    it('should return 200 as status code', async () => {
      // Act
      const response = await server.get('/api/status').expect(200);

      // Assert
      expect(response.status).toBe(200);
    });
  });

  describe('GET /api/swagger', () => {
    it('should return 301 as status code', async () => {
      // Act
      const response = await server.get('/api/swagger').expect(301);

      // Assert
      expect(response.status).toBe(301);
    });
  });
});
