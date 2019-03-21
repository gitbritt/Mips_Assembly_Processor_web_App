using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _222_web_app
{
    public class Answer
    {
        public string
                RegDst = "0",
                ALUSrc = "0",
                MemToReg = "0",
                RegWrite = "0",
                MemRead = "0",
                MemWrite = "0",
                Branch = "0",
                Jump = "0";
        public void Control_Signals(string inst, int rs, int rt, int rd, int shamt, string op_hex_code, string hex_funct, string op_name, string type)
        {
            
            //Determin RegDst/ALUSrc
            if(type == "R")
            {
                RegDst = "1";
                ALUSrc = "0";
            }
            else if(type == "I")
            {
                RegDst = "0";
                ALUSrc = "1";
            }
            if (op_name == "SW" || op_name == "SB" || op_name == "SH" || op_name == "SC")
            {
                MemWrite = "1";
            }
            if (op_name == "LW" || op_name == "LBU" || op_name == "LUBI" || op_name == "LUI")
            {
                MemRead = "1";
                MemToReg = "1";
            }
            if(rd != 0)
            {
                RegWrite = "1";
            }
            if(op_name == "BEQ" || op_name == "BNE")
            {
                Branch = "1";
            }
            if(op_name == "J")
            {
                Jump = "1";
            }
            
        }
    }
}