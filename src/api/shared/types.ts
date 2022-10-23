import { Request } from 'express';
import { LeanDocument, Types } from 'mongoose';

import { UserRole } from '../users/schema/user.schema';

export type RequestWithUser = {
  user: {
    id: string;
    username: string;
    email: string;
    role: UserRole;
  };
} & Request;

export type Id = Types.ObjectId;

export type ObjectId = {
  _id: Id;
};

export type DocumentResponse<T> = LeanDocument<T & ObjectId> | null;

export type QueryAllResponse<T> = {
  data: T[];
  meta: {
    totalDocs: number;
    totalPages: number;
    page: number;
  };
};
