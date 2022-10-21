import winston, { Logger as WinstonLoggerType } from 'winston';

class WinstonLogger {
  private logger: WinstonLoggerType;

  constructor() {
    this.logger = winston.createLogger({
      format: winston.format.combine(
        winston.format.errors({ stacks: true }),
        winston.format.ms(),
        winston.format.prettyPrint(),
        winston.format.simple(),
        winston.format.splat(),
        winston.format.timestamp({
          format: 'DD-MM-YYYY HH:mm:ss',
        }),
        winston.format.printf(
          ({ timestamp, level, message, ms }) =>
            `[${timestamp}] ${level}: ${message} ${ms}`
        ),
        winston.format.colorize({ all: true })
      ),
      transports: [new winston.transports.Console()],
    });
  }

  error(message: string | Error) {
    this.logger.error(message);
  }

  warn(message: string) {
    this.logger.warn(message);
  }

  info(message: string) {
    this.logger.info(message);
  }

  debug(message: string) {
    this.logger.debug(message);
  }
}

const Logger = new WinstonLogger();

export { Logger };
