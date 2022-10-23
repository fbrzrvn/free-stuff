import { QueryAllResponse } from '../shared/types';
import { CreateUserDto, ResponseUserDto, UpdateUserDto } from './dto';
import { userMapper } from './user.mapper';
import { UserRepository } from './user.repository';

class UserService {
  private readonly userRepository: UserRepository;

  constructor() {
    this.userRepository = new UserRepository();
  }

  async create(userData: CreateUserDto): Promise<ResponseUserDto | null> {
    const { email, username } = userData;

    const existingEmail = await this.userRepository.findOne({ email });
    const existingUsername = await this.userRepository.findOne({ username });

    if (Boolean(existingEmail) === true || Boolean(existingUsername) === true) {
      return null;
    }

    await this.userRepository.create({ ...userData });

    const user = await this.userRepository.findOne({ email });

    return userMapper(user);
  }

  async findAll(limit: number, page: number): Promise<QueryAllResponse<ResponseUserDto>> {
    const query = {};

    const totalDocs = await this.userRepository.countDocuments(query);
    const users = await this.userRepository.findAll(limit, page, query);

    return {
      data: users.map(userMapper),
      meta: {
        totalDocs,
        totalPages: Math.ceil(totalDocs / limit),
        page: page
      }
    };
  }

  async findById(id: string): Promise<ResponseUserDto | null> {
    const user = await this.userRepository.findById(id);

    if (Boolean(user) === false) {
      return null;
    }

    return userMapper(user);
  }

  async updateById(id: string, userData: UpdateUserDto): Promise<ResponseUserDto | null> {
    const user = await this.userRepository.updateById(id, userData);

    if (Boolean(user) === false) {
      return null;
    }

    return userMapper(user);
  }

  async deleteOne(id: string): Promise<boolean> {
    const user = await this.userRepository.deleteOne({ _id: id });

    return Boolean(user);
  }
}

export { UserService };
