import { App } from './App';
import { Logger } from './config';

try {
  new App().start();
} catch (error) {
  Logger.error(error as Error);
  process.exit(1);
}

process.on('uncaughtException', (error) => {
  Logger.error(`uncaughtException, ${error}`);
  process.exit(1);
});
