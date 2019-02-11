using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Gekko.Libraries
{
    public static class Password
    {
        /// <summary>
        /// ��������Í�������
        /// </summary>
        /// <param name="str">�Í������镶����</param>
        /// <param name="key">�p�X���[�h</param>
        /// <returns>�Í������ꂽ������</returns>
        public static string Encrypt(string str, string key)
        {
			if ((!string.IsNullOrEmpty(str)) && (!string.IsNullOrEmpty(key)))
			{
				//��������o�C�g�^�z��ɂ���
				byte[] bytesIn = System.Text.Encoding.UTF8.GetBytes(str);

				//DESCryptoServiceProvider�I�u�W�F�N�g�̍쐬
				RijndaelManaged des = new RijndaelManaged();

				//���L�L�[�Ə������x�N�^������
				//�p�X���[�h���o�C�g�z��ɂ���
				byte[] bytesKey = System.Text.Encoding.UTF8.GetBytes(key);
				//���L�L�[�Ə������x�N�^��ݒ�
				des.Key = ResizeBytesArray(bytesKey, des.Key.Length);
				des.IV = ResizeBytesArray(bytesKey, des.IV.Length);

				//�Í������ꂽ�f�[�^�������o�����߂�MemoryStream
				MemoryStream msOut = new MemoryStream();
				//DES�Í����I�u�W�F�N�g�̍쐬
				ICryptoTransform desdecrypt = des.CreateEncryptor();
				//�������ނ��߂�CryptoStream�̍쐬
				CryptoStream cryptStreem = new CryptoStream(msOut, desdecrypt, CryptoStreamMode.Write);
				//��������
				cryptStreem.Write(bytesIn, 0, bytesIn.Length);
				cryptStreem.FlushFinalBlock();
				//�Í������ꂽ�f�[�^���擾
				byte[] bytesOut = msOut.ToArray();

				//����
				cryptStreem.Close();
				msOut.Close();

				//Base64�ŕ�����ɕύX���Č��ʂ�Ԃ�
				return Convert.ToBase64String(bytesOut);
			}
			else
			{
				return string.Empty;
			}
        }

        /// <summary>
        /// �Í������ꂽ������𕜍�������
        /// </summary>
        /// <param name="str">�Í������ꂽ������</param>
        /// <param name="key">�p�X���[�h</param>
        /// <returns>���������ꂽ������</returns>
        public static string Decrypt(string str, string key)
        {
			if ((!string.IsNullOrEmpty(str)) && (!string.IsNullOrEmpty(key)))
			{
				//DESCryptoServiceProvider�I�u�W�F�N�g�̍쐬
				RijndaelManaged des = new RijndaelManaged();

				//���L�L�[�Ə������x�N�^������
				//�p�X���[�h���o�C�g�z��ɂ���
				byte[] bytesKey = Encoding.UTF8.GetBytes(key);
				//���L�L�[�Ə������x�N�^��ݒ�
				des.Key = ResizeBytesArray(bytesKey, des.Key.Length);
				des.IV = ResizeBytesArray(bytesKey, des.IV.Length);

				//Base64�ŕ�������o�C�g�z��ɖ߂�
				byte[] bytesIn = System.Convert.FromBase64String(str);
				//�Í������ꂽ�f�[�^��ǂݍ��ނ��߂�MemoryStream
				MemoryStream msIn = new MemoryStream(bytesIn);
				//DES�������I�u�W�F�N�g�̍쐬
				ICryptoTransform desdecrypt = des.CreateDecryptor();
				//�ǂݍ��ނ��߂�CryptoStream�̍쐬
				CryptoStream cryptStreem = new CryptoStream(msIn, desdecrypt, CryptoStreamMode.Read);

				//���������ꂽ�f�[�^���擾���邽�߂�StreamReader
				StreamReader srOut = new StreamReader(cryptStreem, Encoding.UTF8);
				//���������ꂽ�f�[�^���擾����
				string result = srOut.ReadToEnd();

				//����
				srOut.Close();
				cryptStreem.Close();
				msIn.Close();

				return result;
			}
			else
			{
				return string.Empty;
			}
        }

        /// <summary>
        /// ���L�L�[�p�ɁA�o�C�g�z��̃T�C�Y��ύX����
        /// </summary>
        /// <param name="bytes">�T�C�Y��ύX����o�C�g�z��</param>
        /// <param name="newSize">�o�C�g�z��̐V�����傫��</param>
        /// <returns>�T�C�Y���ύX���ꂽ�o�C�g�z��</returns>
        private static byte[] ResizeBytesArray(byte[] bytes, int newSize)
        {
            byte[] newBytes = new byte[newSize];
            if (bytes.Length <= newSize)
            {
                for (int i = 0; i < bytes.Length; i++)
                    newBytes[i] = bytes[i];
            }
            else
            {
                int pos = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    newBytes[pos++] ^= bytes[i];
                    if (pos >= newBytes.Length)
                        pos = 0;
                }
            }
            return newBytes;
        }
    }
}
