import BruteImage from "./images/Brute.svg";
import CragheartImage from "./images/Cragheart.svg";
import SpellweaverImage from "./images/Spellweaver.svg";
import ScoundrelImage from "./images/Scoundrel.svg";
import TinkererImage from "./images/Tinkerer.svg";
import MindthiefImage from "./images/Mindthief.svg";
import AngryFaceImage from "./images/AngryFace.svg"
import CirclesImage from "./images/Circles.svg"
import EclipseImage from "./images/Eclipse.svg"
import LightningImage from "./images/LightningBolt.svg"
import MusicNotesImage from "./images/MusicNotes.svg"
import SawImage from "./images/Saw.svg"
import SpearsImage from "./images/Spears.svg"
import SquidfaceImage from "./images/Squidface.svg"
import SunImage from "./images/Sun.svg"
import TriforceImage from "./images/Triforce.svg"
import TwoMinisImage from "./images/TwoMinis.svg"

const hpByLevelHigh = [10,12,14,16,18,20,22,24,26];
const hpByLevelMed = [8,9,11,12,14,15,17,18,20];
const hpByLevelLow = [6,7,8,9,10,11,12,13,14]

export const startingCharacters = 
    [
        {Name: "Brute", Image: BruteImage, HpByLevel: hpByLevelHigh},
        {Name: "Cragheart", Image: CragheartImage, HpByLevel: hpByLevelHigh},
        {Name: "Spellweaver", Image: SpellweaverImage, HpByLevel: hpByLevelLow},
        {Name: "Scoundrel", Image: ScoundrelImage, HpByLevel: hpByLevelMed},
        {Name: "Tinkerer", Image: TinkererImage, HpByLevel: hpByLevelMed},
        {Name: "Mindthief", Image: MindthiefImage, HpByLevel: hpByLevelLow}
    ]

export const lockedCharacters = 
    [
        {Name: "Doomstalker", Image: AngryFaceImage, HpByLevel: hpByLevelMed},
        {Name: "Summoner", Image: CirclesImage, HpByLevel: hpByLevelLow},
        {Name: "Nightshroud", Image: EclipseImage, HpByLevel: hpByLevelMed},
        {Name: "Berserker", Image: LightningImage, HpByLevel: hpByLevelHigh},
        {Name: "Sooothsinger", Image: MusicNotesImage, HpByLevel: hpByLevelLow},
        {Name: "Sawbones", Image: SawImage, HpByLevel: hpByLevelMed},
        {Name: "Quartermaster", Image: SpearsImage, HpByLevel: hpByLevelHigh},
        {Name: "Plagueherald", Image: SquidfaceImage, HpByLevel: hpByLevelLow},
        {Name: "Sunkeeper", Image: SunImage, HpByLevel: hpByLevelHigh},
        {Name: "Elementalist", Image: TriforceImage, HpByLevel: hpByLevelLow},
        {Name: "Beast Tyrant", Image: TwoMinisImage, HpByLevel: hpByLevelLow}
    ]

export const apiUrl = 'http://localhost:5148/scenario';

// 'https://si1vct8g53.execute-api.us-east-2.amazonaws.com/Prod'
// 'http://127.0.0.1:3000' 
// http://localhost:5148/scenario
