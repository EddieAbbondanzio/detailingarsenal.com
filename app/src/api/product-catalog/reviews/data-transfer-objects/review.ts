import { Stars } from '../../common/data-transfer-objects/stars';
import { PadCut } from '../../pad-series/data-transfer-objects/pad-cut';
import { PadFinish } from '../../pad-series/data-transfer-objects/pad-finish';

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