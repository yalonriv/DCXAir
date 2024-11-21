import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FlightFilterDTO } from '../DTOs/flight.filter.dto';
import { FlightsService } from '../flights-service.service';
import { Flight } from '../DTOs/flight';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from '../error-dialog/error-dialog.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  providers: [FlightsService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
/**
 * Clase encargada de administrar el componente principal de la pantalla, donde se 
 * puede consultar la información de los vuelos
 */
export class HomeComponent implements OnInit {

  /**
   * Constructor de la clase
   * @param flightService Servicio que permite administrar los vuelos
   * @param dialog  Componente que permite mostrar ventanas modales
   */
  constructor(private flightService: FlightsService, private dialog: MatDialog) { }

  //Atributo que almacenará el tipo de vuelo seleccionado: One way o round trip
  public selectedFlightType = "";
  //Lista que almacena los vuelos consultados
  public flights: Flight[] = [];
  //Lista que almacena los tipos de moneda disponibles
  public coins: string[] = ['USD', 'EUR', 'GBP', 'MXN'];
  //Lista que almacena los origenes disponibles
  public origins: string[];
  //Lista que almacena los destinos disponibles
  public destinations: string[];
  //Dto que guarda los filtros para la búsqueda de los vuelos
  public flightFilterDTO;
  //Lista que almacena los destinos temporalmente
  public temporalDestinations: string[];
  //Atributo que permite saber si se seleccionó o no el destino
  public isSelectedOrigin: boolean = false;
  //Atributo que permite saber si el formulario está completo
  public formIsFilledOut: boolean;

  /**
   * Método que inicializa información e invoca a los métodos que 
   * consultan las listas de origenes y destinos
   */
  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    this.flightFilterDTO = new FlightFilterDTO();
    this.searchOriginFlights();
    this.searchDestinationFlights();
    this.formIsFilledOut = true;
    this.selectedFlightType = 'one-way';
  }

  /**
   * Método encargado de consultar los vuelos, validando si se llenó o 
   * no el formulario para saber si se puede o no realizar la consulta de vuelos.
   * También verifica si se consultarán vuelos de ida y vuelta o solo de ida
   */
  public searchFlight() {
    this.validateForm();
    this.flights = [];
    if (this.formIsFilledOut === true) {
      if (this.selectedFlightType === "one-way") {
        this.searchFlightOneWay();
      }
      else {
        this.searchFlightByRoundTrip();
      }
    }


  }

  /**
   * Método encargado de invocar al servicio searchFlightOneWay de la clase
   * flightService, para consultar el o los vuelos de solo ida
   */
  public searchFlightOneWay() {
    this.flightService.searchFlightOneWay(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;  // Aquí se almacenan los vuelos obtenidos
        console.log('Filtered flights:', this.flights);
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  /**
   * Método encargado de invocar al servicio searchFlightRoundTrip de la clase
   * flightService, para consultar vuelos de ida y vuelta
   */
  public searchFlightByRoundTrip() {
    this.flightService.searchFlightRoundTrip(this.flightFilterDTO).subscribe(
      (response) => {
        this.flights = response;
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  /**
   * Método encargado de consultar los origenes disponibles de los vuelos
   */
  private searchOriginFlights() {
    this.flightService.searchFlightOrigins().subscribe(
      (response) => {
        this.origins = response;
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  /**
   * Método encargado de consultar los origenes disponibles de los vuelos
   */
  private searchDestinationFlights() {
    this.flightService.searchFlightDestinations().subscribe(
      (response) => {
        this.destinations = response;
        this.temporalDestinations = this.destinations;
      },
      (error) => {
        this.showErrorModal(error.message);
        console.error('Error al obtener vuelos:', error);
      }
    );
  }

  /**
   * Método encargado de filtrar los destinos, para que no se muestre un 
   * mismo origen y destino al momento de buscar vuelos
   */
  public updateFilteredOptions() {
    this.isSelectedOrigin = true;
    this.destinations = this.temporalDestinations;
    this.destinations = this.destinations.filter(item => item !== this.flightFilterDTO.origin);


  }

  /**
   * Método encargado de mostrar una ventana modal con un mensaje 
   * de error cuando ocurra alguno
   * @param errorMessage mensaje de error a mostrar
   */
  private showErrorModal(errorMessage: string) {
    this.dialog.open(ErrorDialogComponent, {
      data: errorMessage
    });
  }

  /**
   * 
   */
  private validateForm() {
    if (this.flightFilterDTO.origin === undefined || this.flightFilterDTO.origin === '' || this.flightFilterDTO.origin === null) {
      this.formIsFilledOut = false;
    }
    else {
      this.formIsFilledOut = true;
    }
    if (this.flightFilterDTO.destination === undefined || this.flightFilterDTO.destination === '' || this.flightFilterDTO.destination === null) {
      this.formIsFilledOut = false;
    }
    else {
      this.formIsFilledOut = true;
    }
    if (this.flightFilterDTO.coinToConvert === undefined || this.flightFilterDTO.coinToConvert === '' || this.flightFilterDTO.coinToConvert === null) {
      this.formIsFilledOut = false;
    }
    else {
      this.formIsFilledOut = true;
    }

  }

}
