import { Image } from "../image";
import { PadOption } from "../pad-option";
import { PadOptionCreateOrUpdate } from "./pad-option-create-or-update";

export interface PadColorCreateOrUpdate {
    id: string | null,
    name: string,
    category: string,
    image: Image | string | null,
    options: PadOptionCreateOrUpdate[]
}