import { Response } from 'express';
import passport from 'passport';
import { BadRequestError, Body, HttpCode, JsonController, Post, Res, UseBefore } from 'routing-controllers';
import { OpenAPI } from 'routing-controllers-openapi';

import { HttpException } from '../../exceptions';
import { ValidateMiddleware } from '../../middlewares';
import { ResponseUserDto } from '../users/dto';
import { AuthService } from './auth.service';
import { LoginDto, RegisterDto } from './dto';

@JsonController('/api/auth', { transformResponse: false })
class AuthController {
  private readonly authService: AuthService;

  constructor() {
    this.authService = new AuthService();
  }

  @Post('/register')
  @HttpCode(201)
  @OpenAPI({ summary: 'Register new user' })
  @UseBefore(ValidateMiddleware.validate(RegisterDto))
  async register(@Body() userData: RegisterDto): Promise<ResponseUserDto | HttpException> {
    const user = await this.authService.register(userData);

    if (Boolean(user) === false) {
      throw new BadRequestError('User already exist');
    }

    return user as ResponseUserDto;
  }

  @Post('/login')
  @OpenAPI({ summary: 'Login the user' })
  @UseBefore(ValidateMiddleware.validate(LoginDto))
  async login(@Body() userData: LoginDto, @Res() res: Response): Promise<string | HttpException> {
    const token = await this.authService.loginWithEmailAndPassword(userData);

    if (Boolean(token) === false) {
      throw new BadRequestError('Invalid credentials');
    }

    res.cookie('access_token', token);

    return token as string;
  }

  @Post('/logout')
  @OpenAPI({ summary: 'Logout the user' })
  @UseBefore(passport.authenticate('jwt'))
  async logout(@Res() res: Response) {
    res.clearCookie('access_token');

    return { message: 'Logout success' };
  }

  @Post('/forgot-password')
  @OpenAPI({ summary: 'Send reset token to reset the password' })
  @UseBefore()
  async forgotPassword() {
    // should use email service to send the token to email owner, not return it!
    return { message: 'token to reset the password' };
  }

  @Post('/reset-password')
  @OpenAPI({ summary: 'Reset user password' })
  async resetPassword() {
    return { message: 'password successfully updated' };
  }
}

export { AuthController };
