using Practica2.Models;

namespace Practica2.Repositories.Contracts
{
    public interface IAplicacion

    {
        bool Add(Factura factura);
        List<Factura> GetAll();
        bool Edit(Factura article);
        bool Delete(int id);


    


    }
}
