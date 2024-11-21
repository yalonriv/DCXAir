/**
 * Clase que determina eñ dto que almacena los filtros 
 * de los vuelos a consultar
 */
export class FlightFilterDTO {
    //Determina el origen del vuelo a consultar
    public origin:string;

    //Determina el destino del vuelo
    public destination: string;

    //Determina la moneda en la que se desea consultar
    public coinToConvert: string;
}