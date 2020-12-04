using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimiPro.Validations;

namespace TimiPro.Model
{
    public class ClientEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [ValidaCpf(ErrorMessage = "Este CPF não é valido")]
        public string Cpf { get; set; }

        [ForeignKey("produto")]
        public int ProductId { get; set; }
        public virtual ProductsEntity Product { get; set; }
    }
}
