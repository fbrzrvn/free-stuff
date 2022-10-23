import { AuthController } from './auth/auth.controller';
import { UserController } from './users/user.controller';

const apiControllers: Function[] = [AuthController, UserController];

export { apiControllers };
