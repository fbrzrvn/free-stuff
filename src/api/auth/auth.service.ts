import jwt from 'jsonwebtoken';
import { Env } from '../../constants';
import { ResponseUserDto } from '../users/dto';
import { userMapper } from '../users/user.mapper';
import { UserRepository } from '../users/user.repository';
import { LoginDto, RegisterDto } from './dto';

class AuthService {
  private readonly userRepository: UserRepository;

  constructor() {
    this.userRepository = new UserRepository();
  }

  async register(userData: RegisterDto): Promise<ResponseUserDto | null> {
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

  async loginWithEmailAndPassword(userData: LoginDto): Promise<string | null> {
    const user = await this.userRepository.validate(userData);

    if (Boolean(user) === false) {
      return null;
    }

    const token = jwt.sign(
      {
        id: user?.id,
        username: user?.username,
        email: user?.email,
        role: user?.role
      },
      Env.JWT_SECRET,
      { expiresIn: Env.JWT_EXPIRE }
    );

    return token;
  }
}

export { AuthService };
