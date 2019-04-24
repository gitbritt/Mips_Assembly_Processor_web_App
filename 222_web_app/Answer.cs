
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
                Register_Write_Data = "",
                Memory_Write_Data = "";



        public void Control_Signals(string inst, int rs, int rt, int rd, int shamt, string op_hex_code, string hex_funct, string op_name, string type)
        {

            RegDst = "0";
            ALUSrc = "0";
            MemToReg = "0";
            RegWrite = "0";
            MemRead = "0";
            MemWrite = "0";
            Branch = "0";
            Jump = "0";

            if(type == "R")//Type R
            {
                RegDst = "1";
            }
            else if (op_name == "SW" || op_name == "SB" || op_name == "BEQ" || op_name == "BNE")
            {
                RegDst = "D";
            }
            else if(type == "J")
            {
                RegDst = "D";
            }
            
            if(type == "I")//Type I 
            {
                ALUSrc = "1";
            }
            else if(type == "J")
            {
                ALUSrc = "D";
            }
            
            if(op_name == "LW" || op_name == "LBU")
            {
                MemToReg = "1";
            }
            else if (op_name == "SW" || op_name == "SB" || op_name == "BEQ" || op_name == "BNE")
            {
                MemToReg = "D";
            }

            if (op_name == "AND" || op_name == "ANDI" || op_name == "ADD" || op_name == "ADDI" || op_name == "ADDIU" || op_name == "ADDU" || op_name == "JAL" || op_name == "LBU"
                    || op_name == "LUI" || op_name == "NOR" || op_name == "OR" || op_name == "ORI" || op_name == "SLT" || op_name == "SLTI" || op_name == "SLTIU" || op_name == "SLTU"
                    || op_name == "SLL" || op_name == "SRL" || op_name == "SUBU" || op_name == "SUB" || op_name == "LW")
            {
                RegWrite = "1";
            }

            if(op_name == "LW" || op_name == "LBU")
            {
                MemRead = "1";
            }

            if(op_name == "SW" || op_name == "SB")
            {
                MemWrite = "1";
            }
            if (op_name == "BEQ" || op_name == "BNE")
            {
                Branch = "1";
            }

            if (op_name == "J")
            {
                Jump = "1";
            }

            }
        


        public void Data_Signal(string [] operation_str, string rs_reg, string rt_reg, string rd_reg, int shamt, int immediate, int funct, string location, string jump_addr)
        {
            
            
            ////Getting the Register Numbers from a file//////
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
            
            Read_Register_1 = rt.ToString("X5");
            Read_Register_2 = rs.ToString("X5");


            Read_Data_1 = (rt * 10).ToString("X8");
            Read_Data_2 = (rs * 10).ToString("X8");


            
            string binary_hex = funct.ToString();
            funct = Convert.ToInt32(binary_hex, 16);


            if (operation_str[1] == "R")
            {
                Sign_Ext_Immediate = ((rd << 11) | (shamt << 6) | (funct)).ToString("X8");
                Sign_Ext_Immediate = Sign_Ext_Immediate;
                Write_Register = Convert.ToString(rd, 2);
                if(operation_str[0] == "SLL" || operation_str[0] == "SRL")
                {
                    //Read_Data_1 = (rd * 10).ToString("X8");
                    //Read_Data_2 = (rt * 10).ToString("X8");
                }
            }
            else if (operation_str[1] == "I")
            { 
                string temp = "";
                Sign_Ext_Immediate = immediate.ToString("X8");
                Sign_Ext_Immediate = Sign_Ext_Immediate;

                //Read_Data_1 = (rd * 10).ToString("X8");
                //Read_Data_2 = (rt * 10).ToString("X8");

                if (RegDst != "D")
                    Write_Register = Convert.ToString(rt, 2);
                else
                    Write_Register = "XXXXXXXX"; //If RegDst is a don't care
            }
            else if(operation_str[1] == "J")
            {
                //Help avoid exception issue
                System.Diagnostics.Debug.WriteLine("testing  : " + jump_addr);

                string jump_bin_addr = Convert.ToString(Convert.ToInt64(jump_addr, 16), 2).PadLeft(32, '0'); ;
                System.Diagnostics.Debug.WriteLine("testing  : " + jump_bin_addr + ", " + jump_bin_addr.Length);

                
                Read_Register_1 = jump_bin_addr.Substring(6, 5);
                Read_Register_2 = jump_bin_addr.Substring(11, 5);

                int temp1 = Convert.ToInt32(Read_Register_1, 16) * 10;
                int temp2 = Convert.ToInt32(Read_Register_2, 16) * 10;
                System.Diagnostics.Debug.WriteLine(temp1 + ", " + temp2);
                Read_Data_1 = temp1.ToString("X8");
                Read_Data_2 = temp2.ToString("X8");

                Sign_Ext_Immediate = jump_bin_addr.Substring(16, jump_bin_addr.Length - 16);
                Write_Register = "XXXXXXXX";
            }

            if (operation_str[0] == "SLL" || operation_str[0] == "SRL")
            {

            }

            ////ALU Operation
            ///
            ALU(operation_str[0], operation_str[1], rs, rt, immediate, shamt);

            if (MemToReg == "1")
            {
                Register_Write_Data = "00000000";
                Memory_Write_Data = Read_Data_2;

            }
            else if(MemToReg == "D")
            {
                Register_Write_Data = "00000000";
                Memory_Write_Data = "XXXXXXXX";
            }
            else
            {
                Register_Write_Data = ALU_Result;
                Memory_Write_Data = "00000000";
            }

        }
        public void ALU(string operation_str, string type, int rs, int rt, int immediate, int shamt)
        {
            //ALU Src is add

            ALU_Result = "00000000";
            System.Diagnostics.Debug.WriteLine("ALU op : " + operation_str);
            if (operation_str == "ADD" || operation_str == "ADDI" || operation_str == "ADDIU" || operation_str == "ADDU" || operation_str == "BEQ" || operation_str == "BNE" || operation_str == "JAL" || operation_str == "LUI" || operation_str == "LBU" ||
                operation_str == "LW" || operation_str == "LB" || operation_str == "SB" || operation_str =="SW")
            {
                ALU_Operation = "ADD";
                if(type == "R")
                    ALU_Result =  (rs + rt).ToString("X8");
                else if(type == "I")
                    ALU_Result = (rt + immediate).ToString("X8");
                System.Diagnostics.Debug.WriteLine("ALU add src : " + ALU_Result);
            }
            //ALU Src is or
            if (operation_str == "OR" || operation_str == "ORI" || operation_str == "NOR")
            {
                ALU_Operation = "OR";
                if(type == "R")
                    ALU_Result = (rs | rt).ToString("X8");
                else if(type == "I")
                    ALU_Result = (immediate | rt).ToString("X8");
            }
            if (operation_str == "AND" || operation_str == "ANDI")
            {
                ALU_Operation = "AND";
                if (type == "R")
                    ALU_Result =  (rs & rt).ToString("X8");
                else if (type == "I")
                    ALU_Result = (rs & immediate).ToString("X8");
            }
                
            //ALU is shift right log
            if(operation_str == "SRL")
            {
                ALU_Operation = "SRL";
                ALU_Result = (rt >> shamt).ToString("X8");
            }
            //ALU is shift left log
            if(operation_str == "SLL")
            {
                ALU_Operation = "SLL";
                ALU_Result =  (rt << shamt).ToString("X8");
            }
            //ALU subtract
            if (operation_str == "SUB" || operation_str == "SUBU" || operation_str == "SLT" || operation_str == "SLTI" || operation_str == "SLTIU")
            {
                ALU_Operation = "SUB";
                ALU_Result =  (rt - rs).ToString("X8");
            }
            
        }
    }
}