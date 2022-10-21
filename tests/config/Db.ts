import { MongoMemoryServer } from 'mongodb-memory-server';
import mongoose from 'mongoose';

class Db {
  private static mongoServer: MongoMemoryServer;

  static async connect() {
    this.mongoServer = await MongoMemoryServer.create();
    await mongoose.connect(this.mongoServer.getUri(), {});
  }

  static async close() {
    await mongoose.connection.dropDatabase();
    await mongoose.connection.close();
    await this.mongoServer.stop();
  }

  static async clear() {
    const collections = mongoose.connection.collections;

    for (const key in collections) {
      await collections[key].deleteMany({});
    }
  }
}

export { Db };
