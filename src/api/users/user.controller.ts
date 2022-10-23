import passport from 'passport';
import {
  BadRequestError,
  Body,
  Delete,
  Get,
  HttpCode,
  JsonController,
  NotFoundError,
  Param,
  Post,
  Put,
  QueryParams,
  Req,
  UseBefore
} from 'routing-controllers';
import { OpenAPI } from 'routing-controllers-openapi';

import { HttpException } from '../../exceptions';
import { ValidateMiddleware } from '../../middlewares';
import { IdDto, QueryDto } from '../shared/dto';
import { ValidValue } from '../shared/enums';
import { QueryAllResponse, RequestWithUser } from '../shared/types';
import { CreateUserDto, ResponseUserDto, UpdateUserDto } from './dto';
import { UserService } from './user.service';

@JsonController('/api/users', { transformResponse: false })
@UseBefore(passport.authenticate('jwt'))
class UserController {
  private readonly userService: UserService;

  constructor() {
    this.userService = new UserService();
  }

  @Get('/profile')
  @OpenAPI({ summary: 'Get profile' })
  async get(@Req() req: RequestWithUser): Promise<ResponseUserDto | HttpException> {
    const { id } = req.user;

    const user = await this.userService.findById(id);

    if (Boolean(user) === false) {
      throw new NotFoundError('User not found');
    }

    return user as ResponseUserDto;
  }

  @Put('/profile/update')
  @OpenAPI({ summary: 'Edit profile' })
  @UseBefore(ValidateMiddleware.validate(UpdateUserDto))
  async update(@Req() req: RequestWithUser, @Body() userData: UpdateUserDto): Promise<ResponseUserDto | HttpException> {
    const { id } = req.user;

    const user = await this.userService.updateById(id, userData);

    if (Boolean(user) === false) {
      throw new NotFoundError('User not found');
    }

    return user as ResponseUserDto;
  }

  @Delete('/profile/delete')
  @OpenAPI({ summary: 'Delete profile' })
  async delete(@Req() req: RequestWithUser): Promise<true | HttpException> {
    const { id } = req.user;

    const user = await this.userService.deleteOne(id);

    if (user === false) {
      throw new NotFoundError('User not found');
    }

    return true;
  }

  @Post('/create')
  @HttpCode(201)
  @OpenAPI({ summary: 'Create a new user' })
  @UseBefore(ValidateMiddleware.validate(CreateUserDto))
  async create(@Body() userData: CreateUserDto): Promise<ResponseUserDto | HttpException> {
    const user = await this.userService.create(userData);

    if (Boolean(user) === false) {
      throw new BadRequestError('User already exist');
    }

    return user as ResponseUserDto;
  }

  @Get('/')
  @OpenAPI({ summary: 'Get all users' })
  @UseBefore(ValidateMiddleware.validate(QueryDto, ValidValue.Query))
  async getAll(@QueryParams() query: QueryDto): Promise<QueryAllResponse<ResponseUserDto>> {
    const limit = (query.limit || 50) as string;
    const page = (query.page || 1) as string;

    const users = await this.userService.findAll(+limit, +page);

    return users;
  }

  @Get('/:id')
  @OpenAPI({ summary: 'Get user by id' })
  @UseBefore(ValidateMiddleware.validate(IdDto, ValidValue.Params))
  async getById(@Param('id') id: string): Promise<ResponseUserDto | HttpException> {
    const user = await this.userService.findById(id);

    if (Boolean(user) === false) {
      throw new NotFoundError('User not found');
    }

    return user as ResponseUserDto;
  }

  @Put('/:id/update')
  @OpenAPI({ summary: 'Edit user by id' })
  @UseBefore(ValidateMiddleware.validate(IdDto, ValidValue.Params), ValidateMiddleware.validate(UpdateUserDto))
  async updateById(@Param('id') id: string, @Body() userData: UpdateUserDto): Promise<ResponseUserDto | HttpException> {
    const user = await this.userService.updateById(id, userData);

    if (Boolean(user) === false) {
      throw new NotFoundError('User not found');
    }

    return user as ResponseUserDto;
  }

  @Delete('/:id/delete')
  @OpenAPI({ summary: 'Delete user by id' })
  @UseBefore(ValidateMiddleware.validate(IdDto, ValidValue.Params))
  async deleteOne(@Param('id') id: string): Promise<boolean | HttpException> {
    const user = await this.userService.deleteOne(id);

    if (Boolean(user) === false) {
      throw new NotFoundError('User not found');
    }

    return user;
  }
}

export { UserController };
