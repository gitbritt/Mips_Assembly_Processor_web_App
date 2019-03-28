
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime;

namespace _222_web_app
{
    public class Answer
    {
        static public string
                RegDst = "0",
                ALUSrc = "0",
                MemToReg = "0",
                RegWrite = "0",
                MemRead = "0",
                MemWrite = "0",
                Branch = "0",
                Jump = "0";

        static public string
                Read_Register_1 = "",
                Read_Register_2 = "",
                Write_Register = "",
                Read_Data_1 = "",
                Read_Data_2 = "",
                Sign_Ext_Immediate = "",
                ALU_Operation = "",
                ALU_Result = "",
                Register_Write_Data = "";



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
            else
            {
                RegDst = "0";
                ALUSrc = "0";
            }
            if (op_name == "SW" || op_name == "SB" || op_name == "SH" || op_name == "SC")
            {
                MemWrite = "1";
            }
            else
                MemWrite = "0";
            if (op_name == "LW" || op_name == "LBU" || op_name == "LUBI" || op_name == "LUI")
            {
                MemRead = "1";
                MemToReg = "1";
            }
                else
            {
                MemRead = "0";
                MemToReg = "0";

            }
            if (op_name == "AND" || op_name == "ANDI" || op_name == "ADD" || op_name == "ADDI" || op_name == "ADDIU" || op_name == "ADDU" || op_name == "JAL" || op_name == "LBU"
                || op_name == "LUI" || op_name == "NOR" || op_name == "OR" || op_name == "ORI" || op_name == "SLT" || op_name == "SLTI" || op_name == "SLTIU" || op_name == "SLTU"
                || op_name == "SLL" || op_name == "SRL" || op_name == "SUBU" || op_name == "SUB")
            {
                RegWrite = "1";
            }
            else
                RegWrite = "0";
            if (op_name == "BEQ" || op_name == "BNE")
            {

                Branch = "1";
            }
            else
                Branch = "0";
            if (op_name == "J")
            {
                Jump = "1";
            }
            else
                Jump = "0";
            
        }


        public void Data_Signal(string [] operation_str, string rs_reg, string rt_reg, string rd_reg, int shamt, int immediate, int funct, string location)
        {
            
            
            ////Getting the Register Numbers//////
            StreamReader reg_finder = new StreamReader(location + "Regs.csv");
            string line = "";
            int rs = 0;
            int rt = 0;
            int rd = 0;
            int row = 0;
            
            string row_srt = "";
            while((row_srt = reg_finder.ReadLine()) != null)
            {
                if (rs_reg == row_srt)
                    rs = row - 1;
                if (rt_reg == row_srt)
                    rt = row - 1;
                if (rd_reg == row_srt)
                    rd = row - 1;

                row++;
                
            }
            reg_finder.Close();
            ////Fill in Reg 1 and 2
            ///
            Read_Register_1 = Convert.ToString(rt, 2);
            Read_Register_2 = Convert.ToString(rs, 2);
            
            while(Read_Register_1.Length<5)
            {
                Read_Register_1 = "0" + Read_Register_1;
            }
            while(Read_Register_2.Length<5)
            {
                Read_Register_2 = "0" + Read_Register_2;
            }


            Read_Data_1 = "0x" + (rt * 10).ToString("X8");
            Read_Data_2 = "0x" + (rs * 10).ToString("X8");

            

            string binary_hex = funct.ToString();
            funct = Convert.ToInt32(binary_hex, 16);


            if (operation_str[1] == "R")
            {
                Sign_Ext_Immediate = ((rs << 11) | (shamt << 6) | (funct)).ToString("X8");
                Sign_Ext_Immediate = "0x" + Sign_Ext_Immediate;
                Write_Register = Convert.ToString(rd, 2);

            }
            else if (operation_str[1] == "I")
            { 
                string temp = "";
                Sign_Ext_Immediate = immediate.ToString("X8");
                Sign_Ext_Immediate = "0x" + Sign_Ext_Immediate;

                if (operation_str[0] == "SB" || operation_str[0] == "SW")
                    temp = "0x00000000";
                else
                    temp = Convert.ToString(rt, 2);

                Write_Register = temp;
            }
            else if(operation_str[1] == "J")
            {
                Write_Register = 0.ToString("X8");
            }


            ////ALU Operation
            ///
            ALU(operation_str[0], rs, rt);


        }
        public void ALU(string operation_str, int rs, int rt)
        {
            //ALU Src is add
            if (operation_str.Substring(0, 2) == "ADD" || operation_str == "BEQ" || operation_str == "BNE" || operation_str == "JAL" || operation_str == "LUI" || operation_str == "LBU" ||
                operation_str == "LW" || operation_str == "LB" || operation_str == "SB")
            {
                ALU_Operation = "ADD";
                ALU_Result = "0x" + (rs + rt).ToString("X8");
            }
            //ALU Src is or
            if (operation_str == "OR" || operation_str == "ORI" || operation_str == "NOR")
            {
                ALU_Operation = "OR";
                ALU_Result = "0x" + (rs | rt).ToString("X8");
            }
            //ALU is shift right log
            if(operation_str == "SRL")
            {
                ALU_Operation = "SRL";
            }
            //ALU is shift left log
            if(operation_str == "SLL")
            {
                ALU_Operation = "SLL";
            }
            //ALU subtract
            if (operation_str == "SUB" || operation_str == "SUBU" || operation_str == "SLT" || operation_str == "SLTI" || operation_str == "SLTIU")
            {
                ALU_Operation = "SUB";
                ALU_Result = (rt - rs).ToString("X8");
            }
            else
                ALU_Result = "0x00000000";
        }
    }
}