import { Image } from "../image";
import { PadOption } from "../pad-option";
import { PadOptionUpdate } from "./pad-option-update";

export interface PadColorUpdate {
    name: string,
    category: string,
    image?: Image,
    options: PadOptionUpdate[]
}