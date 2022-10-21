import compression from 'compression';
import cookieParser from 'cookie-parser';
import cors from 'cors';
import express from 'express';
import session from 'express-session';
import ExpressStatusMonitor from 'express-status-monitor';
import helmet from 'helmet';
import hpp from 'hpp';
import http from 'http';
import passport from 'passport';
import 'reflect-metadata';
import { useExpressServer } from 'routing-controllers';

import { apiControllers } from './api';
import { Db, Logger, Swagger } from './config';
import { Env } from './constants';
import {
  ErrorMiddleware,
  JwtPassportMiddleware,
  MorganMiddleware,
} from './middlewares';

class App {
  private express: express.Express;
  private httpServer?: http.Server;
  private controllers: Function[];
  private port = Env.PORT;

  constructor() {
    this.express = express();

    this.express.enable('trust proxy');

    this.express.use(
      ExpressStatusMonitor({
        title: 'Free Stuff api',
        path: '/api/status',
      })
    );

    this.express.use(compression());
    this.express.use(cookieParser());
    this.express.use(cors());
    this.express.use(express.json());
    this.express.use(express.urlencoded({ extended: true }));
    this.express.use(helmet());
    this.express.use(hpp());

    if (Env.NODE !== 'test') {
      this.express.use(MorganMiddleware.onError);
      this.express.use(MorganMiddleware.onSuccess);
    }

    this.express.use(
      session({
        secret: Env.JWT_SECRET,
        resave: false,
        saveUninitialized: false,
      })
    );
    this.express.use(passport.initialize());
    this.express.use(passport.session());

    passport.use(JwtPassportMiddleware);

    this.controllers = apiControllers;

    this._initRoutes(this.controllers);
    this._initSwagger();

    if (Env.NODE !== 'test') {
      Db.connect();
    }
  }

  private _initRoutes(controllers: Function[]): void {
    useExpressServer(this.express, {
      cors: {
        origin: ['http://localhost:3000'],
        credentials: true,
      },
      controllers: controllers,
      defaultErrorHandler: false,
      middlewares: [ErrorMiddleware],
      routePrefix: '/api'
    });
  }

  private _initSwagger(): void {
    new Swagger(this).init();
  }

  public get getControllers(): Function[] {
    return this.controllers;
  }

  public getHTTPServer(): express.Express {
    return this.express;
  }

  public async start(): Promise<void> {
    new Promise(resolve => {
      this.httpServer = this.express.listen(this.port, () => {
        Logger.info(
          `App is running at http://localhost:${
            this.port
          } in ${this.express.get('env')} mode`
        );
        Logger.warn('Press CTRL-C to stop');

        resolve(this.httpServer?.address());
      });
    });
  }

  public async stop(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (this.httpServer) {
        this.httpServer.close(error => {
          if (error) {
            return reject(error);
          }

          return resolve();
        });
      }

      return resolve();
    });
  }
}

export { App };
