import { Stars } from '../../common/data-transfer-objects/stars';
import { PadCut } from '../../pad-series/data-transfer-objects/pad-cut';
import { PadFinish } from '../../pad-series/data-transfer-objects/pad-finish';

export interface ReviewCreateRequest {
    padId: string;
    stars: Stars;
    cut: PadCut | null;
    finish: PadFinish | null;
    title: string;
    body: string;
}