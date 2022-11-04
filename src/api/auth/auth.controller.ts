import { Response } from 'express';
import passport from 'passport';
import {
  BadRequestError,
  Body,
  HttpCode,
  JsonController,
  NotFoundError,
  Params,
  Post,
  Req,
  Res,
  UseBefore
} from 'routing-controllers';
import { OpenAPI } from 'routing-controllers-openapi';

import { ACCESS_TOKEN, REFRESH_TOKEN } from '../../constants';
import { HttpException } from '../../exceptions';
import { ValidateMiddleware } from '../../middlewares';
import { ValidValue } from '../shared/enums';
import { Nullable, RequestWithUser } from '../shared/types';
import { ResponseUserDto } from '../users/dto';
import { AuthService } from './auth.service';
import {
  ForgotPasswordDto,
  LoginDto,
  RefreshTokenDto,
  RegisterDto,
  ResetPasswordDto,
  ResetPasswordParamsDto,
  ResponseTokenDto
} from './dto';

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
  async login(@Body() userData: LoginDto, @Res() res: Response): Promise<ResponseTokenDto | HttpException> {
    const authTokens = await this.authService.loginWithEmailAndPassword(userData);

    if (Boolean(authTokens) === false) {
      throw new BadRequestError('Invalid credentials');
    }

    const { accessToken, refreshToken } = authTokens as ResponseTokenDto;
    const { accessTokenCookieOptions, refreshTokenCookieOptions } = this.authService.generateCookieOptions();

    res.cookie(ACCESS_TOKEN, accessToken, accessTokenCookieOptions);
    res.cookie(REFRESH_TOKEN, refreshToken, refreshTokenCookieOptions);

    return { accessToken, refreshToken };
  }

  @Post('/refresh-token')
  @OpenAPI({ summary: 'Send new access and refresh token' })
  @UseBefore(ValidateMiddleware.validate(RefreshTokenDto))
  async refreshToken(@Body() tokenData: RefreshTokenDto, @Res() res: Response): Promise<ResponseTokenDto> {
    const authTokensResponse = await this.authService.refreshAuthTokens(tokenData.token, tokenData.userId);

    if (authTokensResponse instanceof HttpException) {
      throw authTokensResponse;
    }

    const { accessToken, refreshToken } = authTokensResponse as ResponseTokenDto;
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
  @OpenAPI({ summary: 'Send the link to reset the password' })
  @UseBefore(ValidateMiddleware.validate(ForgotPasswordDto))
  async forgotPassword(@Body() forgotPasswordData: ForgotPasswordDto): Promise<Nullable<string> | HttpException> {
    const linkToResetPassword = await this.authService.forgotPassword(forgotPasswordData.email);

    if (Boolean(linkToResetPassword) === false) {
      throw new NotFoundError(`User not found with email: ${forgotPasswordData.email}`);
    }

    return linkToResetPassword;
  }

  @Post('/reset-password/:id/:token')
  @OpenAPI({ summary: 'Reset user password' })
  @UseBefore(
    ValidateMiddleware.validate(ResetPasswordParamsDto, ValidValue.Params),
    ValidateMiddleware.validate(ResetPasswordDto)
  )
  async resetPassword(
    @Params() resetPasswordParamsDto: ResetPasswordParamsDto,
    @Body() resetPasswordData: ResetPasswordDto
  ): Promise<{ message: string }> {
    const { id, token } = resetPasswordParamsDto;

    const user = await this.authService.resetPassword(id, token, resetPasswordData.password);

    if (user === false) {
      throw new NotFoundError('User not found');
    }

    return { message: 'Password successfully updated' };
  }
}

export { AuthController };
