import bcrypt from 'bcrypt';
import { Document, model, Schema } from 'mongoose';

import { Id } from '../../shared/types';

export enum UserRole {
  Admin = 'admin',
  User = 'user'
}

export interface UserDocument extends Document {
  username: string;
  email: string;
  password: string;
  role: UserRole;
  firstName: string;
  lastName: string;
  birthday: Date;
  avatar: string;
  country: string;
  phone: string;
  location: number[];
  followers: Id[];
  following: Id[];
  hashPassword(password: string): Promise<string>;
  comparePassword(password: string): Promise<boolean>;
}

const UserSchema: Schema = new Schema(
  {
    username: {
      type: String,
      required: [true, 'Username is required'],
      unique: true
    },
    email: {
      type: String,
      required: [true, 'Email is required'],
      unique: true
    },
    password: {
      type: String,
      required: [true, 'Password is required']
    },
    role: {
      type: String,
      enum: UserRole,
      default: UserRole.User
    },
    firstName: {
      type: String
    },
    lastName: {
      type: String
    },
    birthday: {
      type: Date
    },
    avatar: {
      type: String
    },
    country: {
      type: String
    },
    phone: {
      type: String
    },
    location: {
      type: [Number],
      default: []
    },
    followers: {
      type: [Schema.Types.ObjectId],
      ref: 'User',
      default: []
    },
    following: {
      type: [Schema.Types.ObjectId],
      ref: 'User',
      default: []
    }
  },
  {
    timestamps: true
  }
);

UserSchema.pre('save', async function (next) {
  if (this.isModified('password')) {
    this.password = await bcrypt.hash(this.password, bcrypt.genSaltSync(12));
  }

  return next();
});

UserSchema.methods.hashPassword = async function () {
  return await bcrypt.hash(this.password, bcrypt.genSaltSync(12));
};

UserSchema.methods.comparePassword = async function (password: string) {
  return await bcrypt.compare(password, this.password);
};

const UserModel = model<UserDocument>('User', UserSchema);

export { UserModel };
