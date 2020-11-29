import { PadCut } from './pad-cut';
import { PadFinish } from './pad-finish';
import { Stars } from './stars';

export class Review {
    constructor(
        public username: string,
        public date: Date,
        public stars: Stars,
        public cut: PadCut | null,
        public finish: PadFinish | null,
        public title: string,
        public body: string
    ) { }
}