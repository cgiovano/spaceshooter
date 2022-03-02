using SpaceShooter.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter.GameUtils
{
    public static class Patterns
    {
        public static int[] GridPattern(Level level)
        {
            int[] gridPattern = new int[] { };

            switch (level)
            {
                case (Level.Level_1):
                    gridPattern = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case (Level.Level_2):
                    gridPattern = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case (Level.Level_3):
                    gridPattern = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case (Level.Level_4):
                    gridPattern = new int[] { 0, 0, 0, 0, 0, 0, 3, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 3, 3, 3, 3, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2, 2, 1, 1, 1, 1, 1, 1, 2, 2, 0, 0, 0, 0, 0, 2, 2, 0, 0, 1, 1, 1, 1, 0, 0, 2, 2, 0, 0, 0, 0, 2, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                case (Level.Level_5):
                    gridPattern = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                default:
                    break;
            }

            return (gridPattern);
        }

        #region Arrival Patterns
        public static List<Point2D> ArrivePattern_A = new List<Point2D>()
        {
            new Point2D(-15, 390),
            new Point2D(-15, 390),
            new Point2D(90, 390),
            new Point2D(150, 360),
            new Point2D(180, 300),
            new Point2D(160, 240),
            new Point2D(105, 215),
            new Point2D(45, 240),
            new Point2D(30, 295),
            new Point2D(55, 340),
            new Point2D(105, 355),
            new Point2D(155, 300),
            new Point2D(180, 260),
            new Point2D(180, 260)
        };

        public static List<Point2D> ArrivePattern_B = new List<Point2D>()
        {
            new Point2D(280, -5),
            new Point2D(280, -15),
            new Point2D(230, 100),
            new Point2D(130, 175),
            new Point2D(95, 250),
            new Point2D(125, 325),
            new Point2D(185, 345),
            new Point2D(245, 300),
            new Point2D(270, 260),
            new Point2D(270, 260)
        };
        #endregion

        #region Attack Patterns
        public static List<Point2D> AttackPattern_A = new List<Point2D>()
        {
            new Point2D(325, 285),
            new Point2D(325, 285),
            new Point2D(280, 330),
            new Point2D(220, 330),
            new Point2D(175, 285),
            new Point2D(130, 300),
            new Point2D(130, 345),
            new Point2D(175, 360),
            new Point2D(220, 330),
            new Point2D(250, 285),
            new Point2D(295, 300),
            new Point2D(310, 345),
            new Point2D(250, 390),
            new Point2D(145, 405),
            new Point2D(70, 375),
            new Point2D(55, 315),
            new Point2D(85, 285),
            new Point2D(115, 315),
            new Point2D(100, 375),
            new Point2D(55, 420),
            new Point2D(100, 435),
            new Point2D(190, 435),
            new Point2D(280, 405),
            new Point2D(325, 345),
            new Point2D(325, 285),
            new Point2D(325, 285),
        };

        public static List<Point2D> AttackPattern_B = new List<Point2D>()
        {
            new Point2D(135, 300),
            new Point2D(135, 300),
            new Point2D(100, 360),
            new Point2D(160, 420),
            new Point2D(260, 420),
            new Point2D(320, 355),
            new Point2D(305, 275),
            new Point2D(230, 255),
            new Point2D(190, 315),
            new Point2D(235, 375),
            new Point2D(340, 315),
            new Point2D(360, 235),
            new Point2D(360, 235),
        };

        #endregion

        #region Bonus Patterns

        public static Point2D[] BonusStage1_A =
        {
            new Point2D(285, 0),
            new Point2D(285, 0),
            new Point2D(320, 180),
            new Point2D(365, 300),
            new Point2D(430, 370),
            new Point2D(500, 355),
            new Point2D(510, 295),
            new Point2D(420, 235),
            new Point2D(320, 180),
            new Point2D(170, 90),
            new Point2D(-25, -25),
            new Point2D(-25, -25)
        };

        public static Point2D[] BonusStage1_B =
        {
            new Point2D(0, 410),
            new Point2D(0, 410),
            new Point2D(155, 400),
            new Point2D(320, 370),
            new Point2D(435, 315),
            new Point2D(495, 230),
            new Point2D(495, 85),
            new Point2D(445, 50),
            new Point2D(395, 85),
            new Point2D(395, 230),
            new Point2D(445, 265),
            new Point2D(495, 230),
            new Point2D(575, 120),
            new Point2D(640, 0),
            new Point2D(640, 0)
        };
        #endregion

        #region Stage 2 bonus patterns
        public static Point2D[] BonusStage2_A =
        {
            new Point2D(195, 0),
            new Point2D(195, 0),
            new Point2D(195, 90),
            new Point2D(210, 190),
            new Point2D(245, 270),
            new Point2D(315, 345),
            new Point2D(405, 390),
            new Point2D(540, 405),
            new Point2D(645, 405),
            new Point2D(645, 405)
        };

        public static Point2D[] BonusStage2_B =
        {
            new Point2D(-15, 405),
            new Point2D(-15, 405),
            new Point2D(195, 405),
            new Point2D(360, 405),
            new Point2D(465, 345),
            new Point2D(495, 235),
            new Point2D(445, 130),
            new Point2D(320, 85),
            new Point2D(195, 130),
            new Point2D(145, 235),
            new Point2D(175, 345),
            new Point2D(280, 405),
            new Point2D(445, 405),
            new Point2D(640, 405),
            new Point2D(645, 405)
        };
        #endregion

        #region Stage 2 bonus patterns
        public static Point2D[] BonusStage3_A =
        {
            new Point2D(320, -5),
            new Point2D(320, -5),
            new Point2D(320, 130),
            new Point2D(345, 265),
            new Point2D(425, 350),
            new Point2D(540, 385),
            new Point2D(650, 390),
            new Point2D(645, 390)
        };

        public static Point2D[] BonusStage3_B =
        {
            new Point2D(-15, 405),
            new Point2D(-15, 405),
            new Point2D(195, 405),
            new Point2D(295, 405),
            new Point2D(235, 385),
            new Point2D(165, 335),
            new Point2D(135, 255),
            new Point2D(150, 165),
            new Point2D(210, 100),
            new Point2D(295, 75),
            new Point2D(310, 130),
            new Point2D(315, 220),
            new Point2D(330, 240),
            new Point2D(400, 240),
            new Point2D(595, 240),
            new Point2D(650, 240),
            new Point2D(645, 240)
        };
        #endregion

        #region Stage 2 bonus patterns
        public static Point2D[] BonusStage4_A =
        {
            new Point2D(290, -5),
            new Point2D(290, -5),
            new Point2D(320, 20),
            new Point2D(375, 50),
            new Point2D(390, 95),
            new Point2D(370, 140),
            new Point2D(320, 165),
            new Point2D(265, 190),
            new Point2D(245, 240),
            new Point2D(270, 285),
            new Point2D(320, 305),
            new Point2D(375, 330),
            new Point2D(390, 375),
            new Point2D(370, 420),
            new Point2D(320, 435),
            new Point2D(270, 420),
            new Point2D(250, 375),
            new Point2D(265, 330),
            new Point2D(320, 305),
            new Point2D(370, 285),
            new Point2D(395, 245),
            new Point2D(380, 195),
            new Point2D(320, 165),
            new Point2D(275, 145),
            new Point2D(250, 95),
            new Point2D(270, 45),
            new Point2D(320, 20),
            new Point2D(345, -5),
            new Point2D(350, -5)
        };

        public static Point2D[] BonusStage4_B =
        {
            new Point2D(-10, 370),
            new Point2D(-10, 370),
            new Point2D(110, 155),
            new Point2D(200, 85),
            new Point2D(270, 140),
            new Point2D(265, 250),
            new Point2D(205, 290),
            new Point2D(140, 250),
            new Point2D(150, 115),
            new Point2D(285, 75),
            new Point2D(370, 120),
            new Point2D(400, 205),
            new Point2D(385, 310),
            new Point2D(320, 345),
            new Point2D(255, 310),
            new Point2D(240, 205),
            new Point2D(270, 120),
            new Point2D(350, 75),
            new Point2D(435, 85),
            new Point2D(500, 135),
            new Point2D(500, 255),
            new Point2D(435, 290),
            new Point2D(375, 255),
            new Point2D(370, 140),
            new Point2D(435, 85),
            new Point2D(525, 150),
            new Point2D(650, 370),
            new Point2D(650, 360)
        };
        #endregion

        #region Stage 2 bonus patterns
        public static Point2D[] BonusStage5_A =
        {
            new Point2D(270, -10),
            new Point2D(270, -10),
            new Point2D(460, 110),
            new Point2D(510, 240),
            new Point2D(450, 360),
            new Point2D(325, 400),
            new Point2D(195, 360),
            new Point2D(130, 240),
            new Point2D(185, 105),
            new Point2D(300, 60),
            new Point2D(405, 105),
            new Point2D(450, 195),
            new Point2D(420, 285),
            new Point2D(330, 330),
            new Point2D(225, 290),
            new Point2D(195, 195),
            new Point2D(250, 115),
            new Point2D(350, 125),
            new Point2D(395, 195),
            new Point2D(365, 265),
            new Point2D(295, 260),
            new Point2D(275, 205),
            new Point2D(315, 155),
            new Point2D(485, 80),
            new Point2D(645, 40),
            new Point2D(645, 40)
        };

        public static Point2D[] BonusStage5_B =
        {
            new Point2D(-15, 375),
            new Point2D(-15, 375),
            new Point2D(90, 375),
            new Point2D(225, 345),
            new Point2D(360, 240),
            new Point2D(465, 165),
            new Point2D(555, 200),
            new Point2D(555, 310),
            new Point2D(465, 340),
            new Point2D(345, 255),
            new Point2D(185, 165),
            new Point2D(85, 200),
            new Point2D(90, 315),
            new Point2D(185, 335),
            new Point2D(340, 220),
            new Point2D(400, 155),
            new Point2D(400, 70),
            new Point2D(320, 25),
            new Point2D(245, 70),
            new Point2D(250, 170),
            new Point2D(400, 285),
            new Point2D(515, 350),
            new Point2D(645, 375),
            new Point2D(650, 375)
        };
        #endregion

        #region Can Attack Pattern

        public static List<bool> _stage1CanAttackList = new List<bool> { false, false, false, false, false, false, false, false };
        public static List<bool> _stage2CanAttackList = new List<bool> { false, false, false, true, false, false, false, true };
        public static List<bool> _stage3CanAttackList = new List<bool> { false, false, false, true, true, false, false, true };
        public static List<bool> _stage4CanAttackList = new List<bool> { false, true, false, true, true, false, false, true };
        public static List<bool> _stage5CanAttackList = new List<bool> { false, true, false, true, true, false, true, true };

        #endregion
    }
}
