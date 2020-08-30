import { PadCategory } from '@/api';

export type PadCreateOrUpdate = { name: string; category: PadCategory; image?: File };
