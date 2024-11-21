import { Transport } from "./transport";
/**
 * Clase que determina la información del vuelo
 */
export class Flight{
    //Origen del vuelo
    public origin : string;

    //Determina el destino del vuelo
    public destination : string;
   
    //Determina el precio del vuelo
    public price : string;
   
    //Determina el transporte del vuelo
    public transport : Transport;
}