import { Image } from "../image";
import { PadOption } from "../pad-option";
import { PadOptionCreate } from "./pad-option-create";

export interface PadColorCreate {
    name: string,
    category: string,
    image?: Image,
    options: PadOptionCreate[]
}