import mongoose from 'mongoose';

import { Env } from '../constants';
import { Logger } from './Logger';

class Db {
  private static URI: string = Env.DB_URI;

  static connect() {
    if (Env.NODE === 'dev') {
      mongoose.set('debug', true);
    }

    if (Env.DB_USER && Env.DB_PASSWORD && Env.DB_NAME && Env.DB_HOST && Env.NODE === 'prod') {
      Db.URI = `mongodb+srv://${Env.DB_USER}:${encodeURIComponent(
        Env.DB_PASSWORD
      )}@${Env.DB_HOST}/${Env.DB_NAME}?retryWrites=true&w=majority`;
    }

    mongoose.connect(this.URI);

    // When successfully connected
    mongoose.connection.on('connected', () => {
      Logger.info(`Connected to: ${this.URI}`);
    });

    // If the connection throws an error
    mongoose.connection.on('error', error => {
      Logger.error(error);
    });

    // When the connection is disconnected
    mongoose.connection.on('disconnected', () => {
      Logger.info('Mongoose connection disconnected');
    });

    // If the Node process ends, close the Mongoose connection (ctrl + c)
    process.on('SIGINT', () => {
      mongoose.connection.close(() => {
        Logger.warn(
          'Mongoose default connection disconnected through app termination'
        );
        process.exit(0);
      });
    });
  }
}

export { Db };
