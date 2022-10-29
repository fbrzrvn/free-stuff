import { FilterQuery } from 'mongoose';

import { LoginDto, RegisterDto } from '../auth/dto';
import { CreateUserDto, UpdateUserDto } from './dto';
import { UserDocument, UserModel } from './schema/user.schema';

class UserRepository {
  private readonly userModel = UserModel;

  async create(userData: RegisterDto | CreateUserDto) {
    return await this.userModel.create(userData);
  }

  async findAll(limit: number, page: number, query?: any) {
    return await this.userModel
      .find(query)
      .limit(limit)
      .skip(limit * (page - 1))
      .sort({ createdAt: -1 })
      .select('-__v -password -createdAt -updatedAt')
      .lean()
      .exec();
  }

  async findById(id: string) {
    return await this.userModel.findById(id).select('-__v -password -createdAt -updatedAt').lean().exec();
  }

  async findOne(criteria: FilterQuery<UserDocument>) {
    return await this.userModel.findOne(criteria).select('-__v -password -createdAt -updatedAt').lean().exec();
  }

  async updateById(id: string, userData: UpdateUserDto) {
    return await this.userModel
      .findByIdAndUpdate(id, userData, {
        returnOriginal: false
      })
      .select('-__v -password -createdAt -updatedAt')
      .lean()
      .exec();
  }

  async deleteOne(criteria: FilterQuery<UserDocument>) {
    return await this.userModel.findOneAndDelete(criteria).lean().exec();
  }

  async countDocuments(query = {}) {
    return await this.userModel.countDocuments(query);
  }

  async validate(userData: LoginDto) {
    const { email, password } = userData;

    const user = await this.userModel.findOne({ email });

    if (Boolean(user) === false) {
      return null;
    }

    const isPasswordValid = await user?.comparePassword(password);

    if (isPasswordValid === false) {
      return null;
    }

    return user;
  }

  async changePassword(id: string, oldPassword: string, newPassword: string) {
    const user = await this.userModel.findById(id);

    const isPasswordMatch = await user?.comparePassword(oldPassword);

    if (isPasswordMatch === false) {
      return null;
    }

    const hashPassword = await user?.hashPassword(newPassword);

    const updatedUser = await this.userModel
      .findByIdAndUpdate(id, { $set: { password: hashPassword } }, { returnOriginal: false })
      .select('-__v -password -createdAt -updatedAt')
      .lean()
      .exec();

    return updatedUser;
  }
}

export { UserRepository };
