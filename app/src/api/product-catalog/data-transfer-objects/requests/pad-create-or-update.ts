import { Image } from "../image";
import { PadMaterial } from "../pad-material";
import { PadOption } from "../pad-option";
import { PadTexture } from "../pad-texture";
import { PadOptionCreateOrUpdate } from "./pad-option-create-or-update";

export interface PadCreateOrUpdate {
    id: string | null,
    name: string,
    category: string,
    material: PadMaterial,
    texture: PadTexture,
    image: Image | string | null,
    options: PadOptionCreateOrUpdate[]
}