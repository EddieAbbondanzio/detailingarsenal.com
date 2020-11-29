import { PadCut } from '../pad-cut';
import { PadFinish } from '../pad-finish';
import { Stars } from '../stars';

export interface ReviewCreateRequest {
    padId: string;
    stars: Stars;
    cut: PadCut | null;
    finish: PadFinish | null;
    title: string;
    body: string;
}