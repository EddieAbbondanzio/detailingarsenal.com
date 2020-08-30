import { PadCategory } from '@/api';

export type PadCreate = { name: string; category: PadCategory; image?: File };
