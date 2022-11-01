import { Response } from 'express';
import passport from 'passport';
import { BadRequestError, Body, HttpCode, JsonController, Post, Req, Res, UseBefore } from 'routing-controllers';
import { OpenAPI } from 'routing-controllers-openapi';

import { ACCESS_TOKEN, REFRESH_TOKEN } from '../../constants';
import { HttpException } from '../../exceptions';
import { ValidateMiddleware } from '../../middlewares';
import { RequestWithUser } from '../shared/types';
import { ResponseUserDto } from '../users/dto';
import { AuthService } from './auth.service';
import { LoginDto, RefreshTokenDto, RegisterDto } from './dto';

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
  async login(@Body() userData: LoginDto, @Res() res: Response): Promise<any | HttpException> {
    const authTokens = await this.authService.loginWithEmailAndPassword(userData);

    if (Boolean(authTokens) === false) {
      throw new BadRequestError('Invalid credentials');
    }

    const { accessTokenCookieOptions, refreshTokenCookieOptions } = this.authService.generateCookieOptions();

    res.cookie(ACCESS_TOKEN, authTokens.accessToken, accessTokenCookieOptions);
    res.cookie(REFRESH_TOKEN, authTokens.refreshToken, refreshTokenCookieOptions);

    return authTokens;
  }

  @Post('/refresh-token')
  @OpenAPI({ summary: 'Send new access and refresh token' })
  @UseBefore(ValidateMiddleware.validate(RefreshTokenDto))
  async refreshToken(@Body() token: RefreshTokenDto, @Res() res: Response) {
    const { accessToken, refreshToken } = await this.authService.refreshToken(token.token);
    const { accessTokenCookieOptions, refreshTokenCookieOptions } = this.authService.generateCookieOptions();

    res.cookie(ACCESS_TOKEN, accessToken, accessTokenCookieOptions);
    res.cookie(REFRESH_TOKEN, refreshToken, refreshTokenCookieOptions);

    return { accessToken, refreshToken };
  }

  @Post('/logout')
  @OpenAPI({ summary: 'Logout the user' })
  @UseBefore(passport.authenticate('jwt', { session: false }))
  async logout(@Req() req: RequestWithUser, @Res() res: Response): Promise<{ message: string }> {
    const { id } = req.user;

    await this.authService.removeToken(id);

    res.clearCookie(ACCESS_TOKEN);
    res.clearCookie(REFRESH_TOKEN);

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
