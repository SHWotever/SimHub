using System;

namespace SerialDash
{
    internal class Fonts
    {
        public static readonly byte[] FONT_DEFAULT = new byte[]{
            //(byte)Convert.ToInt32( "00000010",2), // (32)  <space>
            (byte)Convert.ToInt32( "00000000",2), // (32)  <space>
           (byte)Convert.ToInt32( "10000110",2), // (33)	!
           (byte)Convert.ToInt32( "00100010",2), // (34)	"
           (byte)Convert.ToInt32( "01111110",2), // (35)	#
           (byte)Convert.ToInt32( "01101101",2), // (36)	$
           (byte)Convert.ToInt32( "00000000",2), // (37)	%
           (byte)Convert.ToInt32( "00000000",2), // (38)	&
           (byte)Convert.ToInt32( "00000010",2), // (39)	'
           (byte)Convert.ToInt32( "00110000",2), // (40)	(
           (byte)Convert.ToInt32( "00000110",2), // (41)	)
           (byte)Convert.ToInt32( "01100011",2), // (42)	*
           (byte)Convert.ToInt32( "00000000",2), // (43)	+
           (byte)Convert.ToInt32( "00000100",2), // (44)	,
           (byte)Convert.ToInt32( "01000000",2), // (45)	-
           (byte)Convert.ToInt32( "10000000",2), // (46)	.
           (byte)Convert.ToInt32( "01010010",2), // (47)	/
           (byte)Convert.ToInt32( "00111111",2), // (48)	0
           (byte)Convert.ToInt32( "00000110",2), // (49)	1
           (byte)Convert.ToInt32( "01011011",2), // (50)	2
           (byte)Convert.ToInt32( "01001111",2), // (51)	3
           (byte)Convert.ToInt32( "01100110",2), // (52)	4
           (byte)Convert.ToInt32( "01101101",2), // (53)	5
           (byte)Convert.ToInt32( "01111101",2), // (54)	6
           (byte)Convert.ToInt32( "00100111",2), // (55)	7
           (byte)Convert.ToInt32( "01111111",2), // (56)	8
           (byte)Convert.ToInt32( "01101111",2), // (57)	9
           (byte)Convert.ToInt32( "00000000",2), // (58)	:
           (byte)Convert.ToInt32( "00000000",2), // (59)	;
           (byte)Convert.ToInt32( "00000000",2), // (60)	<
           (byte)Convert.ToInt32( "01001000",2), // (61)	=
           (byte)Convert.ToInt32( "00000000",2), // (62)	>
           (byte)Convert.ToInt32( "01010011",2), // (63)	?
           (byte)Convert.ToInt32( "01011111",2), // (64)	@
           (byte)Convert.ToInt32( "01110111",2), // (65)	A
           (byte)Convert.ToInt32( "01111111",2), // (66)	B
           (byte)Convert.ToInt32( "00111001",2), // (67)	C
           (byte)Convert.ToInt32( "00111111",2), // (68)	D
           (byte)Convert.ToInt32( "01111001",2), // (69)	E
           (byte)Convert.ToInt32( "01110001",2), // (70)	F
           (byte)Convert.ToInt32( "00111101",2), // (71)	G
           (byte)Convert.ToInt32( "01110110",2), // (72)	H
           (byte)Convert.ToInt32( "00000110",2), // (73)	I
           (byte)Convert.ToInt32( "00011111",2), // (74)	J
           (byte)Convert.ToInt32( "01101001",2), // (75)	K
           (byte)Convert.ToInt32( "00111000",2), // (76)	L
           (byte)Convert.ToInt32( "00010101",2), // (77)	M
           (byte)Convert.ToInt32( "00110111",2), // (78)	N
           (byte)Convert.ToInt32( "00111111",2), // (79)	O
           (byte)Convert.ToInt32( "01110011",2), // (80)	P
           (byte)Convert.ToInt32( "01100111",2), // (81)	Q
           (byte)Convert.ToInt32( "00110001",2), // (82)	R
           (byte)Convert.ToInt32( "01101101",2), // (83)	S
           (byte)Convert.ToInt32( "01111000",2), // (84)	T
           (byte)Convert.ToInt32( "00111110",2), // (85)	U
           (byte)Convert.ToInt32( "00101010",2), // (86)	V
           (byte)Convert.ToInt32( "00011101",2), // (87)	W
           (byte)Convert.ToInt32( "01110110",2), // (88)	X
           (byte)Convert.ToInt32( "01101110",2), // (89)	Y
           (byte)Convert.ToInt32( "01011011",2), // (90)	Z
           (byte)Convert.ToInt32( "00111001",2), // (91)	[
           (byte)Convert.ToInt32( "01100100",2), // (92)	\ (this can't be the last char on a line, even in comment or it'll concat)
           (byte)Convert.ToInt32( "00001111",2), // (93)	]
           (byte)Convert.ToInt32( "00000000",2), // (94)	^
           (byte)Convert.ToInt32( "00001000",2), // (95)	_
           (byte)Convert.ToInt32( "00100000",2), // (96)	`
           (byte)Convert.ToInt32( "01011111",2), // (97)	a
           (byte)Convert.ToInt32( "01111100",2), // (98)	b
           (byte)Convert.ToInt32( "01011000",2), // (99)	c
           (byte)Convert.ToInt32( "01011110",2), // (100)	d
           (byte)Convert.ToInt32( "01111011",2), // (101)	e
           (byte)Convert.ToInt32( "00110001",2), // (102)	f
           (byte)Convert.ToInt32( "01101111",2), // (103)	g
           (byte)Convert.ToInt32( "01110100",2), // (104)	h
           (byte)Convert.ToInt32( "00000100",2), // (105)	i
           (byte)Convert.ToInt32( "00001110",2), // (106)	j
           (byte)Convert.ToInt32( "01110101",2), // (107)	k
           (byte)Convert.ToInt32( "00110000",2), // (108)	l
           (byte)Convert.ToInt32( "01010101",2), // (109)	m
           (byte)Convert.ToInt32( "01010100",2), // (110)	n
           (byte)Convert.ToInt32( "01011100",2), // (111)	o
           (byte)Convert.ToInt32( "01110011",2), // (112)	p
           (byte)Convert.ToInt32( "01100111",2), // (113)	q
           (byte)Convert.ToInt32( "01010000",2), // (114)	r
           (byte)Convert.ToInt32( "01101101",2), // (115)	s
           (byte)Convert.ToInt32( "01111000",2), // (116)	t
           (byte)Convert.ToInt32( "00011100",2), // (117)	u
           (byte)Convert.ToInt32( "00101010",2), // (118)	v
           (byte)Convert.ToInt32( "00011101",2), // (119)	w
           (byte)Convert.ToInt32( "01110110",2), // (120)	x
           (byte)Convert.ToInt32( "01101110",2), // (121)	y
           (byte)Convert.ToInt32( "01000111",2), // (122)	z
           (byte)Convert.ToInt32( "01000110",2), // (123)	{
           (byte)Convert.ToInt32( "00000110",2), // (124)	|
           (byte)Convert.ToInt32( "01110000",2), // (125)	}
           (byte)Convert.ToInt32( "00000001",2), // (126)	~
           (byte)Convert.ToInt32( "00000000",2), // (127)	
            (byte)Convert.ToInt32( "00000000",2), // (128)	
            (byte)Convert.ToInt32( "00000000",2), // (129)	
            (byte)Convert.ToInt32( "00000000",2), // (130)	
            (byte)Convert.ToInt32( "00000000",2), // (131)	
            (byte)Convert.ToInt32( "00000000",2), // (132)	
            (byte)Convert.ToInt32( "00000000",2), // (133)	
            (byte)Convert.ToInt32( "00000000",2), // (134)	
            (byte)Convert.ToInt32( "00000000",2), // (135)	
            (byte)Convert.ToInt32( "00000000",2), // (136)	
            (byte)Convert.ToInt32( "00000000",2), // (137)	
            (byte)Convert.ToInt32( "00000000",2), // (138)	
            (byte)Convert.ToInt32( "00000000",2), // (139)	
            (byte)Convert.ToInt32( "00000000",2), // (140)	
            (byte)Convert.ToInt32( "00000000",2), // (141)	
            (byte)Convert.ToInt32( "00000000",2), // (142)	
            (byte)Convert.ToInt32( "00000000",2), // (143)	
            (byte)Convert.ToInt32( "00000000",2), // (144)	
            (byte)Convert.ToInt32( "00000000",2), // (145)	
            (byte)Convert.ToInt32( "00000000",2), // (146)	
            (byte)Convert.ToInt32( "00000000",2), // (147)	
            (byte)Convert.ToInt32( "00000000",2), // (148)	
            (byte)Convert.ToInt32( "00000000",2), // (149)	
            (byte)Convert.ToInt32( "00000000",2), // (150)	
            (byte)Convert.ToInt32( "00000000",2), // (151)	
            (byte)Convert.ToInt32( "00000000",2), // (152)	
            (byte)Convert.ToInt32( "00000000",2), // (153)	
            (byte)Convert.ToInt32( "00000000",2), // (154)	
            (byte)Convert.ToInt32( "00000000",2), // (155)	
            (byte)Convert.ToInt32( "00000000",2), // (156)	
            (byte)Convert.ToInt32( "00000000",2), // (157)	
            (byte)Convert.ToInt32( "00000000",2), // (158)	
            (byte)Convert.ToInt32( "00000000",2), // (159)	
            (byte)Convert.ToInt32( "00000000",2), // (160)	
            (byte)Convert.ToInt32( "00000000",2), // (161)	
            (byte)Convert.ToInt32( "00000000",2), // (162)	
            (byte)Convert.ToInt32( "00000000",2), // (163)	
            (byte)Convert.ToInt32( "00000000",2), // (164)	
            (byte)Convert.ToInt32( "00000000",2), // (165)	
            (byte)Convert.ToInt32( "00000000",2), // (166)	
            (byte)Convert.ToInt32( "00000000",2), // (167)	
            (byte)Convert.ToInt32( "00000000",2), // (168)	
            (byte)Convert.ToInt32( "00000000",2), // (169)	
            (byte)Convert.ToInt32( "00000000",2), // (170)	
            (byte)Convert.ToInt32( "00000000",2), // (171)	
            (byte)Convert.ToInt32( "00000000",2), // (172)	
            (byte)Convert.ToInt32( "00000000",2), // (173)	
            (byte)Convert.ToInt32( "00000000",2), // (174)	
            (byte)Convert.ToInt32( "00000000",2), // (175)	
            (byte)Convert.ToInt32( "01100011",2), // (176)	°
            (byte)Convert.ToInt32( "00000000",2), // (177)	
            (byte)Convert.ToInt32( "00000000",2), // (178)	
            (byte)Convert.ToInt32( "00000000",2), // (179)	
        };
    }
}