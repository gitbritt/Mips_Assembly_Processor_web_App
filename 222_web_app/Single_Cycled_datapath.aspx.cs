﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace _222_web_app
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Get random function
            try
            {
                string[] register_array = { "$t0", "$t1", "$t2", "$t3", "$t4", "$t5", "$t6", "$t7", "$t8", "$t9", "$s0", "$s1", "$s2", "$s3", "$s3", "$s4", "$s5", "$s6", "$s7", "$ra" };
                int rs = 0;
                int rt = 0;
                int rd = 0;
                int shamt = 0;
                int immediate = 0;
                int register_array_length = register_array.Length;
                int address = 0;
                int Branch_Address = 0;
                Random operation = new Random();
                int operation_row_file = operation.Next(2, 27);
                string home_path = Server.MapPath("/").ToString();
                string reg_file_path = home_path + "/Register_file/";
                string row = File.ReadLines(reg_file_path + "instructions.csv").Skip(operation_row_file - 1).Take(1).First();
                string[] operation_str = row.Split(',');
                string instruction = "";
                string hex_address = "";
                string rs_reg = "";
                string rt_reg = "";
                string rd_reg = "";
                ////Type R instructions
                ///
                if (operation_str[1] == "R")
                {
                    rs = operation.Next(0, register_array_length);
                    rt = operation.Next(0, register_array_length);
                    rd = operation.Next(0, register_array_length);
                    rs_reg = register_array[rs];
                    rt_reg = register_array[rt];
                    rd_reg = register_array[rd];
                    if(operation_str[0] == "SRL" || operation_str[0] == "SLL")
                    {
                        shamt = operation.Next(0, 10);
                        instruction = operation_str[0] + " " + rs_reg + ", " + rt_reg + ", " + shamt;
                    }
                    else if(operation_str[0] == "JR")
                    {

                        instruction = operation_str[0] + " " + register_array[19];
                    }
                    else
                        instruction = operation_str[0] + " " + rs_reg + ", " + rt_reg + ", " + rd_reg;
                    
                }
                ///Type I instructions
                ///
                else if(operation_str[1] =="I")
                {
                    immediate = operation.Next(0, 10);
                    instruction = operation_str[0];
                    rs = operation.Next(0, register_array_length);
                    rt = operation.Next(0, register_array_length);
                    rs_reg = register_array[rs];
                    rt_reg = register_array[rt];
                    //// Load/Store 
                    if (instruction == "SB" || instruction == "SW" || instruction == "LW" || instruction == "LUI" || instruction == "LB" || instruction == "LUBI" || instruction == "LUB")
                    {
                        instruction = instruction + " " + rs_reg + ", " + immediate + "( " + rt_reg + " )";
                    }
                    ////Branches
                    ///
                    else if(operation_str[0] == "BNE" || operation_str[0] == "BEQ")
                    {
                        Branch_Address = operation.Next(0, 65535);
                        hex_address = Branch_Address.ToString("X");
                        instruction = instruction + " " + register_array[rs] + ", " + register_array[rt] + ", " + "0x" + hex_address;
                    }
                    ////All other type of instructions
                    ///
                    else
                        instruction = instruction + " " + rs_reg + ", " + rt_reg + ", " + immediate;
                    
                }
                ////Type J instructions
                ///
                else if(operation_str[0] == "J")
                {
                    address = operation.Next(0, 8388607);
                    hex_address = address.ToString("X");

                    instruction = operation_str[0] + " " + "0x" + hex_address;
                    
                }
                //Append HTML to add instruction
                Instruction_html.InnerText = instruction;

                ////Set Correct Answer
                ///
                Answer ans = new Answer();
                
                int funct = Convert.ToInt32(operation_str[3], 16);
                ans.Control_Signals(instruction, rs, rt, rd, shamt, operation_str[2], operation_str[3], operation_str[0], operation_str[1]);
                ans.Data_Signal(operation_str, rs_reg, rt_reg, rd_reg, shamt, immediate, funct , reg_file_path);
            }
            catch (Exception ex)
            {
                Instruction_html.InnerText = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected void Submit_Answers_Click(object sender, EventArgs e)
        {
            Answer ans = new Answer();
            AnswerRegDst.InnerText = Answer.RegDst;
            AnswerALUSrc.InnerText = Answer.ALUSrc;
            AnswerMemtoReg.InnerText = Answer.MemToReg;
            AnswerRegWrite.InnerText = Answer.RegWrite;
            AnswerMemRead.InnerText = Answer.MemRead;
            AnswerMemWrite.InnerText = Answer.MemWrite;
            AnswerBranch.InnerText = Answer.Branch;
            AnswerJump.InnerText = Answer.Jump;

            AnswerReadReg1.InnerText = Answer.Read_Register_1;
            AnswerReadReg2.InnerText = Answer.Read_Register_2;
            AnswerWriteReg.InnerText = Answer.Write_Register;
            AnswerReadData1.InnerText = Answer.Read_Data_1;
            AnswerReadData2.InnerText = Answer.Read_Data_2;
            AnswerSignImmediate.InnerText = Answer.Sign_Ext_Immediate;
            AnswerALUOperation.InnerText = Answer.ALU_Operation;
            AnswerALUResult.InnerText = Answer.ALU_Result;
            AnswerRegWriteData.InnerText = Answer.Register_Write_Data;
        }
    }
}