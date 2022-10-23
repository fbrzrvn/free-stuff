import { IsMongoId } from 'class-validator';

import { Id } from '../types';

class IdDto {
  @IsMongoId()
  readonly id: Id;
}

export { IdDto };
