import { PadCategory } from '@/api';

export type PadCreateRequest = { name: string; category: PadCategory; image?: File };
