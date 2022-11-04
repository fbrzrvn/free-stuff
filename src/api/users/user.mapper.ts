import { DocumentResponse } from '../shared/types';
import { ResponseUserDto } from './dto';
import { UserDocument, UserRole } from './schema/user.schema';

const userMapper = (user: DocumentResponse<UserDocument>): ResponseUserDto => {
  const mappedUser: ResponseUserDto = {
    id: user?._id,
    username: user?.username as string,
    email: user?.email as string,
    role: user?.role as UserRole,
    firstName: user?.firstName,
    lastName: user?.lastName,
    avatar: user?.avatar,
    country: user?.country,
    phone: user?.phone,
    location: user?.location,
    followers: user?.followers,
    following: user?.following
  };

  return mappedUser;
};

export { userMapper };
