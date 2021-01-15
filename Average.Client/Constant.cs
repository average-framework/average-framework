using Client.Core.Enums;
using Client.Models;
using System.Collections.Generic;
using static Shared.Core.DataModels.CharacterData;

namespace Client
{
    public static class Constant
    {
        public static string ResourceName { get; } = "avg";

        #region Character

        public static List<PedOverlay> OverlaysInfo = new List<PedOverlay>
        {
            new PedOverlay("eyebrows", 0x07844317, 0xF81B2E66, 0x7BC4288B, 0x202674A1),
            new PedOverlay("eyebrows", 0x0A83CA6E, 0x8FA4286B, 0xBD811948, 0xB82C8FBB),
            new PedOverlay("eyebrows", 0x139A5CA3, 0x487ABE5A, 0x22A9DDF9, 0x78AA9401),
            new PedOverlay("eyebrows", 0x1832E474, 0x96FBB931, 0x32FA2683, 0xA1775B18),
            new PedOverlay("eyebrows", 0x216EF84C, 0x269CD8F8, 0x2F54C727, 0xCCBD1939),
            new PedOverlay("eyebrows", 0x2594304D, 0xA5A23CD1, 0x8611B42C, 0x0238302B),
            new PedOverlay("eyebrows", 0x33C39BC5, 0xF928E29B, 0x46C268BD, 0x4B92F13E),
            new PedOverlay("eyebrows", 0x443E3CBA, 0x6C83B571, 0x2B191070, 0xD551E623),
            new PedOverlay("eyebrows", 0x4F5052DE, 0x827EEF46, 0x70E8C702, 0xD97518F9),
            new PedOverlay("eyebrows", 0x5C049D35, 0x41E90506, 0x7E47D163, 0x54100288),
            new PedOverlay("eyebrows", 0x77A1546E, 0x43C4AE44, 0x290FC7F7, 0xD8FC26A9),
            new PedOverlay("eyebrows", 0x8A4B79C2, 0xAE6ED4E6, 0x89B29E5A, 0xFA0476E4),
            new PedOverlay("eyebrows", 0x9728137B, 0x23E65D35, 0xEE39073F, 0x218DD4C8),
            new PedOverlay("eyebrows", 0xA6DE8325, 0x7A93F649, 0x22B33B65, 0xEE6CCF11),
            new PedOverlay("eyebrows", 0xA8CCB6C4, 0x29AD8BF9, 0x34ABB09D, 0xCF206860),
            new PedOverlay("eyebrows", 0xB3F74D19, 0x3E2F71B1, 0xD4809D11, 0x9ABFA640),
            new PedOverlay("eyebrows", 0xBD38AFD9, 0x058A698E, 0x9A732F86, 0x2EF1D769),
            new PedOverlay("eyebrows", 0xCD0A4F7C, 0xED46998E, 0xB5B73A38, 0x15C5FB78),
            new PedOverlay("eyebrows", 0xD0EC86FF, 0x81B462A2, 0x894F8744, 0x51551810),
            new PedOverlay("eyebrows", 0xEB088A20, 0x0C6CDBDC, 0x91A2496E, 0xE639F138),
            new PedOverlay("eyebrows", 0xF0CA96FC, 0xAC3BCA3F, 0x667FEFF8, 0xDD8E5EFF),
            new PedOverlay("eyebrows", 0xF3351BD9, 0xC3286EA4, 0x8BB9158A, 0xFBBAE4D8),
            new PedOverlay("eyebrows", 0xF9052779, 0x8AEADE78, 0x21BB2D97, 0x75A0B928),
            new PedOverlay("eyebrows", 0xFE183197, 0x92B508CD, 0x6AA92A3E, 0xB4A436DB),

            new PedOverlay("scars", 0xC8E45B5B, 0x6245579F, 0xD53A336F),
            new PedOverlay("scars", 0x90D86B44, 0xA1538E6F, 0xDFCB1159),
            new PedOverlay("scars", 0x23190FC3, 0x39683ECE, 0x249C1A0A),
            new PedOverlay("scars", 0x7574B47D, 0x3AB2A0BB, 0x7A70886A),
            new PedOverlay("scars", 0x7FE8C965, 0xB81C8D16, 0x7210971B),
            new PedOverlay("scars", 0x083059FE, 0xC332710C, 0x860EE45E),
            new PedOverlay("scars", 0x19E9FD71, 0x40895310, 0xB753C5C7),
            new PedOverlay("scars", 0x4CAF62FB, 0xD80F2F64, 0x00BBF225),
            new PedOverlay("scars", 0xDE650668, 0x85F6BF71, 0x3DD0B0AE),
            new PedOverlay("scars", 0xC648562B, 0x6397E4D9, 0x2B59CDA1),
            new PedOverlay("scars", 0x484BAEF8, 0xBF2946DE, 0xD3F2F2F6),
            new PedOverlay("scars", 0x190F5080, 0xCBBDB741, 0x9518FA34),
            new PedOverlay("scars", 0x2B5DF51D, 0x0E05C415, 0x8B8C57AC),
            new PedOverlay("scars", 0xE490E784, 0x50853115, 0xDA7F2A1E),
            new PedOverlay("scars", 0x0ED23C06, 0xAEA45D76, 0x364DAAA6),
            new PedOverlay("scars", 0x5712CCB6, 0x9318AF61, 0x98104C8C),

            new PedOverlay("eyeliners", 0x29A2E58F, 0xA952BF75, 0xDD55AF2A),
            new PedOverlay("lipsticks", 0x887E11E0, 0x96A5E4FB, 0x1C77591C, 0x4255A5F4),
            new PedOverlay("acne", 0x96DD8F42, 0x1BA4244B, 0xBA46CE92),
            new PedOverlay("shadows", 0x47BD7289, 0x5C5C98FC, 0xE20345CC),
            new PedOverlay("beardstabble", 0x375D4807, 0xB5827817, 0x5041B648, 0x83F42340),
            new PedOverlay("paintedmasks", 0x5995AA6F, 0x99BCB03F),

            new PedOverlay("ageing", 0x96DD8F42, 0x1BA4244B, 0xBA46CE92),
            new PedOverlay("ageing", 0x6D9DC405, 0xAFE82F0C, 0x5CF8808E),
            new PedOverlay("ageing", 0x2761B792, 0x4105C6B3, 0x8607CC56),
            new PedOverlay("ageing", 0x19009AD0, 0xEBC18618, 0x9087AF96),
            new PedOverlay("ageing", 0xC29F6E07, 0xF9887FA7, 0x1331C3C9),
            new PedOverlay("ageing", 0xA45F3187, 0x1C30961A, 0x3CA2F3AE),
            new PedOverlay("ageing", 0x5E21250C, 0x01E35044, 0x5A965FF0),
            new PedOverlay("ageing", 0x4FFE08C6, 0xA65757F2, 0xC46CC005),
            new PedOverlay("ageing", 0x2DAD4485, 0x358DEFDA, 0x55D317B4),
            new PedOverlay("ageing", 0x3F70680B, 0x7073A58F, 0x33E73C5F),
            new PedOverlay("ageing", 0xD3310F8E, 0xD9E8A605, 0x22297EA5),
            new PedOverlay("ageing", 0xF27A4C84, 0xE0F0971B, 0x9F0E6718),
            new PedOverlay("ageing", 0x0044E819, 0xFD844ADF, 0x315A6D56),
            new PedOverlay("ageing", 0xA648348D, 0xC329F765, 0xE8CD7F20),
            new PedOverlay("ageing", 0x94F991F0, 0x8586D19B, 0xCA334396),
            new PedOverlay("ageing", 0xCAACFD56, 0xD2D0BF4F, 0xE0203BDA),
            new PedOverlay("ageing", 0xB9675ACB, 0x2387AF71, 0x90A80AE1),
            new PedOverlay("ageing", 0x3C2CE03C, 0xC6DCBCCA, 0x609B7EBD),
            new PedOverlay("ageing", 0xF2D64D90, 0xC6DCBCCA, 0x609B7EBD),
            new PedOverlay("ageing", 0xE389AEF7, 0xDF591FF2, 0x11D92A14),
            new PedOverlay("ageing", 0x89317A44, 0xB4640D19, 0x2F56FDA5),
            new PedOverlay("ageing", 0x64B3347C, 0xFF2E8F96, 0x45EE7B10),
            new PedOverlay("ageing", 0x9FFDAB10, 0x8F2950D9, 0x85BDD7E8),
            new PedOverlay("ageing", 0x91D40EBD, 0x5DCD1D4E, 0xA1B5F71F),
            new PedOverlay("ageing", 0x6B94C23F, 0xF17FE41C, 0x0C480977),

            new PedOverlay("blush", 0x6DB440FA, 0x43B1AACA),
            new PedOverlay("blush", 0x47617455, 0x9CAD2EF0),
            new PedOverlay("blush", 0x114D082D, 0xA52E3B98),
            new PedOverlay("blush", 0xEC6F3E72, 0xB5CED4CB),

            new PedOverlay("complex", 0xF679EDE7, 0xFAAE9FF0),
            new PedOverlay("complex", 0x3FFB80ED, 0x1FDFD4A1),
            new PedOverlay("complex", 0x31C0E478, 0xC72D0698),
            new PedOverlay("complex", 0x2457C9A6, 0x98F1C76F),
            new PedOverlay("complex", 0x16262D43, 0xE0D03293),
            new PedOverlay("complex", 0x88F312DB, 0x2ECCC670),
            new PedOverlay("complex", 0x785C71AE, 0xAE1C329F),
            new PedOverlay("complex", 0x6D7D5BF0, 0x23201E55),
            new PedOverlay("complex", 0x5F2FBF55, 0x94503F97),
            new PedOverlay("complex", 0xBF38FF6A, 0x5F62F986),
            new PedOverlay("complex", 0xF5656C26, 0x83417009),
            new PedOverlay("complex", 0x03A408A3, 0x1BCC4185),
            new PedOverlay("complex", 0x293453C3, 0x6C556574),
            new PedOverlay("complex", 0x43150800, 0x1E486F85),

            new PedOverlay("disc", 0xD44A5ABA, 0x2D3AEB2F),
            new PedOverlay("disc", 0xE2CF77C4, 0xB8945AC0),
            new PedOverlay("disc", 0xCF57D0E9, 0xB15E4E47),
            new PedOverlay("disc", 0xE0A8738A, 0x25A711DD),
            new PedOverlay("disc", 0xABD109DC, 0xCEBED6D9),
            new PedOverlay("disc", 0xB91C2472, 0xFDD6C9AB),
            new PedOverlay("disc", 0x894844B7, 0x7E89B165),
            new PedOverlay("disc", 0x96FAE01C, 0x458799CD),
            new PedOverlay("disc", 0x86D3BFCE, 0x8F2F2826),
            new PedOverlay("disc", 0x5488DB39, 0xB49A0275),
            new PedOverlay("disc", 0x7DA5A5AE, 0x8200F51D),
            new PedOverlay("disc", 0xE73778DC, 0x8D35AC90),
            new PedOverlay("disc", 0xD83EDADF, 0x96B619CD),
            new PedOverlay("disc", 0xE380F163, 0xAB7309F7),
            new PedOverlay("disc", 0xB4611324, 0x26FEBDD4),
            new PedOverlay("disc", 0xC6ABB7B9, 0xC162C835),

            new PedOverlay("foundation", 0xEF5AB280, 0xD9264247, 0x1535C7C9),

            new PedOverlay("freckles", 0x1B794C51, 0x59B8159A),
            new PedOverlay("freckles", 0x29BFE8DE, 0x03FCF67B),
            new PedOverlay("freckles", 0x0EF6B34C, 0x21E2FD82),
            new PedOverlay("freckles", 0x64925E7E, 0x3FD45844),
            new PedOverlay("freckles", 0xF5F280FC, 0xE372E00E),
            new PedOverlay("freckles", 0x33B0FC78, 0x288810E0),
            new PedOverlay("freckles", 0x25675FE5, 0xEB8C0B1D),
            new PedOverlay("freckles", 0xD10F3736, 0x3885AC2A),
            new PedOverlay("freckles", 0x5126B75F, 0xB061C984),
            new PedOverlay("freckles", 0x6B8EEC2F, 0xE1D1113E),
            new PedOverlay("freckles", 0x0A9A26F7, 0xA1EC1AEA),
            new PedOverlay("freckles", 0xFDE40D8B, 0x6DBC9203),
            new PedOverlay("freckles", 0x7E338E44, 0x097D1D0A),
            new PedOverlay("freckles", 0x70F273C2, 0x81A25BCE),
            new PedOverlay("freckles", 0x61C7D56D, 0x197A1335),

            new PedOverlay("grime", 0xA2F30923, 0x16CDD724, 0x136165B3, 0xF3DFA7AC),
            new PedOverlay("grime", 0xD5B1EEA0, 0x0E599D69, 0x5C67FB68, 0x40FEC59E),
            new PedOverlay("grime", 0x7EC740CC, 0x0FAE8DC6, 0x9E7A4B63, 0xB48BF65A),
            new PedOverlay("grime", 0xB08F245B, 0x98358521, 0x1FAA4A84, 0x81428E8F),
            new PedOverlay("grime", 0x1A5E77F8, 0x8D3D2563, 0x1FAA4A84, 0x81428E8F),
            new PedOverlay("grime", 0xE81B9373, 0xAE43378D, 0x0CBEEF9B, 0x92097B22),
            new PedOverlay("grime", 0x3CFA3D2F, 0x7499570E, 0xA27FF667, 0x24B49749),
            new PedOverlay("grime", 0x0B865A48, 0xB80F6B12, 0x377319E3, 0x3CDC25A9),
            new PedOverlay("grime", 0x506DE416, 0x537BA522, 0x006AF092, 0x5CCEA9F8),
            new PedOverlay("grime", 0x1F250185, 0x51BE975D, 0x3F718027, 0x5527ACCF),
            new PedOverlay("grime", 0xE71930B0, 0x595D09A3, 0xF4E08D43, 0x60B91CE7),
            new PedOverlay("grime", 0xDE571F2C, 0xE7FAFDFA, 0xE6A18BBF, 0xCB315A57),
            new PedOverlay("grime", 0x0CA6FBCB, 0x0E27372E, 0xD4894921, 0xBF339D56),
            new PedOverlay("grime", 0x21F62669, 0x693623F0, 0xDB95176C, 0xEA27B375),
            new PedOverlay("grime", 0xFB09D881, 0xC4A40DA0, 0xADD1DC3D, 0xFD797A87),
            new PedOverlay("grime", 0x11530513, 0x67C6D30F, 0x26AA38C3, 0x89C2FFE3),

            new PedOverlay("hair", 0x39051515, 0x60A4A360, 0x8D65EFF2, 0x62759D82),
            new PedOverlay("hair", 0x5E71DFEE, 0x71147B90, 0xD8EB57BC),
            new PedOverlay("hair", 0xDD735DEF, 0x493214E4, 0x6613D121),
            new PedOverlay("hair", 0x69622EAD, 0xA6E819C4, 0xE581D851),

            new PedOverlay("moles", 0x821FD077, 0xDFDA0798, 0xE4E90C92),
            new PedOverlay("moles", 0xCD38E6A8, 0xE9CF623E, 0x43FAEA4B),
            new PedOverlay("moles", 0x9F9D8B72, 0x27450B2F, 0x0808DBFB),
            new PedOverlay("moles", 0xE7179A39, 0x38638E0B, 0x99346057),
            new PedOverlay("moles", 0xBB094249, 0x763F8624, 0x6975D6F9),
            new PedOverlay("moles", 0x03AC5362, 0xEF158115, 0xBA297751),
            new PedOverlay("moles", 0x154FF6A9, 0xEE28E6F7, 0xB7548307),
            new PedOverlay("moles", 0x1E23084F, 0x566ACE2F, 0x361237C6),
            new PedOverlay("moles", 0x31DBAFC0, 0x0AB0CC2B, 0xDBF55701),
            new PedOverlay("moles", 0x3AC5C194, 0xC940CC25, 0x41CB48FC),
            new PedOverlay("moles", 0x4500D516, 0x3A1EEDB1, 0x17BC19B0),
            new PedOverlay("moles", 0x3695B840, 0x1D30222E, 0xDA5FDF7E),
            new PedOverlay("moles", 0x286C1BED, 0x4F0B4FA8, 0x40333534),
            new PedOverlay("moles", 0x934BF1AF, 0x4540A8D7, 0x933ACF76),
            new PedOverlay("moles", 0x84F55502, 0x47BE6D32, 0xDCF7108E),
            new PedOverlay("moles", 0xBD9A464B, 0x9DABB1B9, 0x4A3B1739),

            new PedOverlay("spots", 0x5BBFF5F7, 0x24968425, 0xA5D532AD),
            new PedOverlay("spots", 0x65EC0A4F, 0x326A7845, 0xC09B2354),
            new PedOverlay("spots", 0x3F143CA0, 0x91D7E39E, 0xD607DF75),
            new PedOverlay("spots", 0x49675146, 0x2E6C3769, 0xE6A21CD5),
            new PedOverlay("spots", 0x07504D2D, 0x39F16CE6, 0x5CB32D5C),
            new PedOverlay("spots", 0xF161214F, 0x47C60FBA, 0x19424C77),
            new PedOverlay("spots", 0xE43286F2, 0xA7E86379, 0x7C07E0B0),
            new PedOverlay("spots", 0xDDDC7A46, 0x26D3DA64, 0x5A69A9BB),
            new PedOverlay("spots", 0xD086DF9B, 0x7D6FF58C, 0x5A0D99C8),
            new PedOverlay("spots", 0xBA51B331, 0xCB23CA55, 0xA7720C6A),
            new PedOverlay("spots", 0xE4CF097B, 0x51D0FBDA, 0xB01F5202),
            new PedOverlay("spots", 0xF70CADF6, 0xD0858DFC, 0x7E067837),
            new PedOverlay("spots", 0xC07F40DC, 0x3BAF1008, 0x75030E1B),
            new PedOverlay("spots", 0xD3B1E741, 0x97091388, 0xA191AA56),
            new PedOverlay("spots", 0xB494A903, 0x18025AE1, 0x86F51AD1),
            new PedOverlay("spots", 0xC6EE4DB6, 0xC9F3EBA4, 0xE819AD33),
        };
        public static List<OverlayData> OverlayLayers = new List<OverlayData>
        {
            new OverlayData("eyebrows"),
            new OverlayData("scars"),
            new OverlayData("eyeliners"),
            new OverlayData("lipsticks"),
            new OverlayData("acne"),
            new OverlayData("shadows"),
            new OverlayData("beardstabble"),
            new OverlayData("paintedmasks"),
            new OverlayData("ageing"),
            new OverlayData("blush"),
            new OverlayData("complex"),
            new OverlayData("disc"),
            new OverlayData("foundation"),
            new OverlayData("freckles"),
            new OverlayData("grime"),
            new OverlayData("hair"),
            new OverlayData("moles"),
            new OverlayData("spots"),
        };
        public static List<PedComponent> PedComponents { get; } = new List<PedComponent>
        {
            new PedComponent()
            {
                Gender = Gender.Male,
                Heads = new List<string> { "A0BE4A7B", "1E78F6D", "27A4DC22", "5E9A394D", "7BE9E352", "7D7AA347", "839997EF", "84F3E485", "876B1FAE" },
                Body = new List<string> { "206061DB", "16E292A1", "206061DB", "4AEEDD87", "4BD8F4A1", "BA59624D", "F6496128" },
                Legs = new List<string> { "84BAA309", "8AD16414", "A615E02", "E4B584D5", "F11CF1FA", "F2EA7BDE" },
                HeadTexture = "mp_head_mr1_sc08_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Male,
                Heads = new List<string> { "3625908B", "40365810", "613CF195", "66B5DE24", "7AFEF216", "8574F682" },
                Body = new List<string> { "3825D527", "5A929214", "82F14D87", "99E82863", "DF522C3A" },
                Legs = new List<string> { "15ACBB1D", "97596A2A", "EA27EDE2", "F91940A6", "9EE7070B", "D7F34979" },
                HeadTexture = "mp_head_mr1_sc02_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Male,
                Heads = new List<string> { "158CB7F2", "36739C03", "48133466", "4E11220E", "68C7CDA8", "6DF5043C", "6EF1C97C", "771A7EE9", "7B035098" },
                Body = new List<string> { "465F719A", "6D582255", "8CC97681", "B0D24F3F" },
                Legs = new List<string> { "47FE9FC0", "64F9856B", "B0F62B29", "ACAE409" },
                HeadTexture = "mp_head_mr1_sc03_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Male,
                Heads = new List<string> { "4C5C14D1", "2BADE2F9", "32E0BD65", "51EE52F8", "53361205", "7150F35A", "7320223C", "8036DB0B", "8239BA1C" },
                Body = new List<string> { "34C3B131", "5C1686B", "8C9686C8", "BF787383" },
                Legs = new List<string> { "F5E0272A", "B897BFA0", "C17616E", "73710076", "B897BFA0" },
                HeadTexture = "mp_head_fr1_sc01_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Male,
                Heads = new List<string> { "1EF1D4F5", "20BEAD17", "17E48A5C", "1D1391CB", "3E1D8D10", "41FB09E2", "421209B8", "44C938AE", "465D3511", "48531C43", "5A5A4569", "69A6DC4D", "87198A9F" },
                Body = new List<string> { "4824ED39", "3B6F503", "5EA984F8", "B4485D23", "6BB6BC48" },
                Legs = new List<string> { "D3A7708B", "5B835093", "DDF48A5D", "35D1FB67", "364F2807" },
                HeadTexture = "mp_head_mr1_sc04_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Male,
                Heads = new List<string> { "101E374E", "47A369D9", "48A3A1FC", "4C55A1AB", "5248AA25", "52CC549C", "54CED1F4", "6817A7D2", "6B50E776", "729570C7" },
                Body = new List<string> { "69B6305B", "BA09D8ED", "C8EA5978", "CD7F8895" },
                Legs = new List<string> { "52CC3F25", "6577142C", "6CBCE93C", "887C4C70", "EF9D2DAE" },
                HeadTexture = "mp_head_fr1_sc05_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Female,
                Heads = new List<string> { "76ACA91E", "30378AB3", "478C7817", "6D06466A", "772F8047", "7C1A194E", "87311A4B", "18665C91", "1B15AE7A", "20F6540D" },
                Body = new List<string> { "489AFE52", "64181923", "8DCF7A49", "928DAD43", "B1D3B3A", "D878696D" },
                Legs = new List<string> { "11A244CC", "3B653032", "41021120", "A0736DA7", "C3BFA017", "F0CD92EC" },
                HeadTexture = "mp_head_fr1_sc08_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Female,
                Heads = new List<string> { "1C851DA8", "2E1791E1", "477D749A", "62534D55", "6D8686E8", "87371192", "886DB564", "8A1E0CED" },
                Body = new List<string> { "8002D0F8", "6C25B6F6", "8223BCC5", "2BE27CC4", "C1CF0BC1" },
                Legs = new List<string> { "18916A9B", "4D38CBC5", "98975DF3", "DC1AD9D2", "E74007F9" },
                HeadTexture = "mp_head_fr1_sc02_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Female,
                Heads = new List<string> { "1E6FDDFB", "30B5C9FA", "43857351", "50A1A9F2", "5A274672", "6369FC85", "65A5CE70", "6DCBE781", "47BC4C6" },
                Body = new List<string> { "2C4FE0C5", "3708268F", "7145337D", "79D35251", "B2850A03" },
                Legs = new List<string> { "3E152D7E", "C9903FE8", "CC543A45", "24CF58B7", "1684BC22" },
                HeadTexture = "mp_head_fr1_sc03_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Female,
                Heads = new List<string> { "11567C3", "1D896D8D", "24452D0B", "34FC0B13", "43F08B06", "53B5B98F", "5DC6A042", "5F192A74", "7F2AAA30", "89B0F7FE" },
                Body = new List<string> { "5B4E1547", "58D8EA30", "87363366", "D0C5A9AE", "DC86C81" },
                Legs = new List<string> { "CEAB4EC0", "DBE4E491", "5B4E1547", "F9609455", "3DBEB4BD" },
                HeadTexture = "mp_head_fr1_sc05_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Female,
                Heads = new List<string> { "11F10982", "2AE6E5C", "4A52F943", "50882CFA"/*, ""*/, "65F9F637", "6A0AB89D", "8E53BDC1" },
                Body = new List<string> { "80DB09DE", "93925FA2", "94778799", "56617DB6", "E36416C" },
                Legs = new List<string> { "31BE7295", "71821457", "9D6CD26", "A29CE6D7", "A65CF97E" },
                HeadTexture = "mp_head_fr1_sc01_c0_000_ab"
            },
            new PedComponent()
            {
                Gender = Gender.Female,
                Heads = new List<string> { "11F69034", "169B95C6", "16C5E95A", "1C32EE08", "22B4E685", "3129C6F1", "3C7D04E4", "40E72684", "75AF6E83", "8A7F3F41", "8CD1ABC6", "544D8D50" },
                Body = new List<string> { "35A7C9FB", "1B088705", "A1AEFBDB", "E28C4D3B", "C05A25AD" },
                Legs = new List<string> { "27B700C2", "4BA188D", "AAD70276", "EDE17D5F", "F4F5A364" },
                HeadTexture = "mp_head_fr1_sc04_c0_000_ab"
            }
        };

        #endregion

        #region Configs

        public static dynamic Config { get; set; }
        public static List<Models.Ped> Peds { get; set; }
        public static List<Models.DoorInfo> DoorsInfos { get; set; }
        public static List<Models.Imap> Imaps { get; set; }
        public static List<Models.MyImap> MyImaps { get; set; }
        public static List<Models.Interior> Interiors { get; set; }
        public static List<Models.InteriorSet> InteriorsSet { get; set; }
        public static List<Models.MyInterior> MyInteriors { get; set; }
        public static Models.Clothing Clothes { get; set; }

        #endregion
    }
}
