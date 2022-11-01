import dotenv from 'dotenv';

dotenv.config();

class Env {
  static readonly DB_HOST: string = process.env.DB_HOST as string;
  static readonly DB_NAME: string = process.env.DB_NAME as string;
  static readonly DB_PASSWORD: string = process.env.DB_PASSWORD as string;
  static readonly DB_PORT: string = process.env.DB_PORT as string;
  static readonly DB_URI: string = process.env.DB_URI as string;
  static readonly DB_USER: string = process.env.DB_USER as string;
  static readonly JWT_ACCESS_EXPIRE: string = process.env.JWT_ACCESS_EXPIRE as string;
  static readonly JWT_REFRESH_EXPIRE: string = process.env.JWT_REFRESH_EXPIRE as string;
  static readonly JWT_RESET_EXPIRE: string = process.env.JWT_RESET_EXPIRE as string;
  static readonly JWT_SECRET: string = process.env.JWT_SECRET as string;
  static readonly NODE: string = process.env.NODE_ENV as string;
  static readonly PORT: string | number = process.env.PORT || 3000;
}

export { Env };
