using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;

namespace Gert.Email
{
    public class EnvioEmail
    {
        public Configuracoes Configuracoes { get; set; }

        private SmtpClient MontarCliente()
        {
            try
            {
                var cliente = new SmtpClient()
                {
                    Host = this.Configuracoes.Smtp,
                    EnableSsl = this.Configuracoes.SslEmail,
                    Credentials = new NetworkCredential(this.Configuracoes.LoginEmail, this.Configuracoes.SenhaEmail),
                    Port = Convert.ToInt16(this.Configuracoes.PortaEmail)
                };

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível montar o cliente SMTP", ex);
            }
        }

        public void EnviarEmailAlunosTarefa(Tarefa tarefa)
        {
            try
            {
                var cliente = MontarCliente();

                var disciplina = GertDbFactory.Instance.DisciplinaRepository.FindById(tarefa.Disciplina.Id);

                var message = new MailMessage()
                {
                    IsBodyHtml = false,
                    Priority = MailPriority.High,
                    From = new MailAddress(this.Configuracoes.LoginEmail, disciplina.Professor.Nome),
                    Sender = new MailAddress(this.Configuracoes.LoginEmail, disciplina.Professor.Nome),
                    Subject = "Nova Tarefa - " + tarefa.Disciplina.Nome,
                    Body = "Nova Tarefa - " + tarefa.Disciplina.Nome +"\n\n" +
                           "O professor "+ disciplina.Professor.Nome +"acabou de cadastrar um nova tarefa de " + tarefa.Disciplina.Nome + ".\n\n"+
                           "Data de Início: " + tarefa.DtInicio.ToShortDateString() + "\n" +
                           "Data de Fim: " + tarefa.DtFinal.ToShortDateString() + "\n\n\n"+
                           "Obs.: Este e-mail foi enviado de forma automática pela plataforma GERT, por favor, não responda."
                };

                foreach (var aluno in tarefa.Disciplina.DisciplinaAlunos)
                {
                    if (!string.IsNullOrEmpty(aluno.Aluno.Email))
                    {
                        message.To.Add(new MailAddress(aluno.Aluno.Email, aluno.Aluno.Nome));
                    }
                }

                cliente.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível enviar o e-mail", ex);
            }
        }
    }
}
