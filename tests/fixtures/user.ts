import { LoginDto, RegisterDto } from '../../src/api/auth/dto';
import { ResponseUserDto } from '../../src/api/users/dto';
import { UserRole } from '../../src/api/users/schema/user.schema';

const getRegisterDto = (username?: string, email?: string, password?: string): RegisterDto => {
  const user: RegisterDto = {
    username: username || 'lerelle',
    email: email || 'lerelle@email.com',
    password: password || 'lerelle'
  };

  return user;
};

const getLoginDto = (email?: string, password?: string): LoginDto => {
  const user: LoginDto = {
    email: email || 'lerelle@email.com',
    password: password || 'lerelle'
  };

  return user;
};

const mappedUser: Omit<ResponseUserDto, 'id'> = {
  username: 'lerelle',
  email: 'lerelle@email.com',
  role: UserRole.User,
  location: [],
  followers: [],
  following: []
};

export { getLoginDto, getRegisterDto, mappedUser };
