using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoTeste.Domain.Entities;
using MercadoLibre.SDK;

namespace IntegracaoTeste.Application.Contracts
{
    public interface IQuestionAnswerApp
    {
        QuestionAnswer GetQuestionAnswerBySellerId(Meli m, string Id);
    }
}
