﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mostrarSegundaSeñal(false);
        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e)
        {
            
            double tiempoInicial =
                double.Parse(txtTiempoInicial.Text);
            double tiempoFinal =
                double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo =
                double.Parse(txtFrecuenciaMuestreo.Text);


            Señal señal;
            Señal segundaSeñal = null;
            Señal señalResultante;

            switch(cbTipoSeñal.SelectedIndex)
            {
                case 0: // Parabolica
                    señal = new SeñalParabolica();
                
                
                    break;
                case 1: //Senoidal
                    double amplitud =
                        double.Parse(
                    ((ConfiguracionSeñalSenoidal)
                        (panelConfiguracion.Children[0])).txtAmplitud.Text);
                    double fase =
                        double.Parse(
                            ((ConfiguracionSeñalSenoidal)
                            (panelConfiguracion.Children[0])).txtFase.Text
                            );
                    double frecuencia =
                        double.Parse(
                            ((ConfiguracionSeñalSenoidal)
                            (panelConfiguracion.Children[0])).txtFrecuencia.Text
                            );
                    señal = 
                        new SeñalSenoidal(amplitud, fase, frecuencia);
                    
                    break;
                case 2:
                    string rutaArchivo =
                        ((ConfiguracionAudio)
                        (panelConfiguracion.Children[0])).txtRutaArchivo.Text;
                    señal = new SeñalAudio(rutaArchivo);
                    txtTiempoInicial.Text =
                        señal.TiempoInicial.ToString();
                    txtTiempoFinal.Text =
                        señal.TiempoFinal.ToString();
                    txtFrecuenciaMuestreo.Text =
                        señal.FrecuenciaMuestreo.ToString();
                    break;
                default:
                    señal = null;
                    break;
            }

            

            if (cbTipoSeñal.SelectedIndex != 2 
                && señal != null)
            {
                señal.TiempoInicial =
                        tiempoInicial;
                señal.TiempoFinal =
                    tiempoFinal;
                señal.FrecuenciaMuestreo =
                    frecuenciaMuestreo;

                señal.construirSeñal();
            }
                        
            //Construir segunda señal si es necesario
            if (cbOperacion.SelectedIndex == 2 | cbOperacion.SelectedIndex == 4)
            {
                switch (cbTipoSeñal_2.SelectedIndex)
                {
                    case 0: // Parabolica
                        segundaSeñal = new SeñalParabolica();
                        break;
                    case 1: //Senoidal
                        double amplitud =
                            double.Parse(
                        ((ConfiguracionSeñalSenoidal)
                            (panelConfiguracion_2.Children[0])).txtAmplitud.Text);
                        double fase =
                            double.Parse(
                                ((ConfiguracionSeñalSenoidal)
                                (panelConfiguracion_2.Children[0])).txtFase.Text
                                );
                        double frecuencia =
                            double.Parse(
                                ((ConfiguracionSeñalSenoidal)
                                (panelConfiguracion_2.Children[0])).txtFrecuencia.Text
                                );
                        segundaSeñal =
                            new SeñalSenoidal(amplitud, fase, frecuencia);

                        break;
                    case 2:
                        string rutaArchivo =
                            ((ConfiguracionAudio)
                            (panelConfiguracion_2.Children[0])).txtRutaArchivo.Text;
                            segundaSeñal = new SeñalAudio(rutaArchivo);
                        txtTiempoInicial.Text =
                            segundaSeñal.TiempoInicial.ToString();
                        txtTiempoFinal.Text =
                            segundaSeñal.TiempoFinal.ToString();
                        txtFrecuenciaMuestreo.Text =
                            segundaSeñal.FrecuenciaMuestreo.ToString();
                        break;
                    default:
                        segundaSeñal = null;
                        break;
                }
                if (cbTipoSeñal_2.SelectedIndex != 2 && segundaSeñal != null)
                {
                    segundaSeñal.TiempoInicial = tiempoInicial;
                    segundaSeñal.TiempoFinal = tiempoFinal;
                    segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;

                    segundaSeñal.construirSeñal();
                }
            }


            switch(cbOperacion.SelectedIndex)
            {
                case 0: //Escala de amplitud
                    double factorEscala =
                        double.Parse(
                        ((OperacionEscalaAmplitud)
                        (panelConfiguracionOperacion.
                            Children[0])).txtFactorEscala
                            .Text);
                    señalResultante =
                        Señal.escalarAmplitud(señal,
                        factorEscala);
                    break;
                case 1:
                    double factorDesplazamiento =
                        double.Parse(((DesplazamientoAmplitud)(panelConfiguracionOperacion.Children[0]))
                        .txtFactorDesplazamiento.Text);
                    señalResultante =  Señal.desAmplitud(señal,factorDesplazamiento);
                    break;
                case 2:
                    señalResultante = Señal.multiplicarSeñales(señal, segundaSeñal);

                    break;
                case 3:
                    double factorExponencial =
                       double.Parse(((OperacionEscalaExponencial)(panelConfiguracionOperacion.Children[0]))
                       .txtFactorExponente.Text);
                    señalResultante = Señal.escalaExponencial(señal, factorExponencial);                    
                    break;
                case 4:
                    señalResultante = Señal.adicionarSeñales(señal, segundaSeñal);
                    break;
                case 5://Transf. de Fourier
                    señalResultante = Señal.transformadaFourier(señal);
                    break;
                default:
                    señalResultante = null;
                    break;
            }
            /////AQUI ME QUEDE
            

           
            //Operador ternario
            //Evalua condicion. Si si y Si no
            //Elige entre la primera y la resultante
            double amplitudMaxima =
                (señal.AmplitudMaxima >= señalResultante.AmplitudMaxima) ?
                señal.AmplitudMaxima : señalResultante.AmplitudMaxima;
            //Elige entre la mas grande de la 1ra y resultante y la segunda 
            if (segundaSeñal != null)
            {
                amplitudMaxima = (amplitudMaxima > segundaSeñal.AmplitudMaxima) ?
                    amplitudMaxima : segundaSeñal.AmplitudMaxima;
            }

            


            plnGrafica.Points.Clear();
            plnGraficaResultante.Points.Clear();
            plnGrafica_2.Points.Clear();

            if (segundaSeñal != null)
            {

                foreach (var muestra in segundaSeñal.Muestras)
                {
                    plnGrafica_2.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
                }
            }

            foreach (Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(
                    adaptarCoordenadas(muestra.X,
                    muestra.Y, tiempoInicial, amplitudMaxima)
                    );
            }

            foreach(Muestra muestra in señalResultante.Muestras)
            {
                plnGraficaResultante.Points.Add(
                    adaptarCoordenadas(muestra.X,
                    muestra.Y,tiempoInicial,amplitudMaxima)
                    );

            }

            if (cbOperacion.SelectedIndex == 5)
            {   //Ontenemos la frecuencia baja
                int indiceMaximo = 0;
                int indiceInicial = (int)((690.0 * (float)(señalResultante.Muestras.Count)) / señalResultante.FrecuenciaMuestreo);
                int indiceFinal = (int)((950.0 * (float)(señalResultante.Muestras.Count)) / señalResultante.FrecuenciaMuestreo);

                for (int i = indiceInicial; i < (indiceFinal); i++)
                {
                    if(señalResultante.Muestras[i].Y > señalResultante.Muestras[indiceMaximo].Y)
                    {
                        indiceMaximo = i;
                    }
                }
                lblHertzBaja.Text = ((indiceMaximo * señalResultante.FrecuenciaMuestreo) / (señal.Muestras.Count)).ToString( "N") + "Hz";
                //Obtenemos la frecuenca alta
                int indiceMaximoAlta = 0;
                int indiceInicialAlta = ((int)((1200.0 * (float)(señalResultante.Muestras.Count)) / señalResultante.FrecuenciaMuestreo));
                int indiceFinalAlta = ((int)((1482.0 * (float)(señalResultante.Muestras.Count)) / señalResultante.FrecuenciaMuestreo));
                 
                for (int i = indiceInicialAlta; i < indiceFinalAlta; i++)
                {
                    if (señalResultante.Muestras[i].Y > señalResultante.Muestras[indiceMaximoAlta].Y)
                    {
                        indiceMaximoAlta = i;
                    }
                }
                lblHertzAlta.Text = ((indiceMaximoAlta * señalResultante.FrecuenciaMuestreo) / (señal.Muestras.Count)).ToString("N") + "Hz";

                double hertzAlta = ((indiceMaximoAlta * señalResultante.FrecuenciaMuestreo) / (señal.Muestras.Count));
                double hertzBaja = ((indiceMaximo* señalResultante.FrecuenciaMuestreo) / (señal.Muestras.Count));

                //lblNumero.Text = hertzAlta.ToString("N");

                if ((hertzBaja <= (697.0f)) && (hertzAlta <= 1212.0f))
                {
                    lblNumero.Text = "1";
                }
                else if (hertzBaja <= 697.0f && hertzAlta <= 1336.0f)
                {
                    lblNumero.Text = "2";
                }
                else if (hertzBaja <= 697.0f && hertzAlta <= 1477.0f)
                {
                    lblNumero.Text = "3";
                }
                else if (hertzBaja <= 770.0f && hertzAlta <= 1209.0f)
                {
                    lblNumero.Text = "4";
                }
                else
                    if (hertzBaja <= 770.0f && hertzAlta <= 1336.0f)
                {
                    lblNumero.Text = "5";
                }
                else  if (hertzBaja <= 770.0f && hertzAlta <= 1477.0f)
                {
                    lblNumero.Text = "6";
                }
                else if (hertzBaja <= 852.0f && hertzAlta <= 1209.0f)
                {
                    lblNumero.Text = "7";
                }
                else if (hertzBaja <= 852.0f && hertzAlta <= 1336.0f)
                {
                    lblNumero.Text = "8";
                }
                else if (hertzBaja <= 852.0f && hertzAlta <= 1477.0f)
                {
                    lblNumero.Text = "9";
                }
                else if (hertzBaja <= 941.0f && hertzAlta <= 1209.0f)
                {
                    lblNumero.Text = "*";
                }
                else if (hertzBaja <= 941.0f && hertzAlta <= 1336.0f)
                {
                    lblNumero.Text = "0";
                }
                else
                {
                    lblNumero.Text = "#";
                }



            }
           
            lblLimiteSuperior.Text =
                amplitudMaxima.ToString("F");
            lblLimiteInferior.Text =
                "-" + amplitudMaxima.ToString("F");
                        
            lblLimiteSuperiorResultante.Text =  
                amplitudMaxima.ToString("F");
            lblLimiteInferiorResultante.Text = "-" +
                amplitudMaxima.ToString("F");

            
            //Original
            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(
                adaptarCoordenadas(tiempoInicial, 0.0,
                tiempoInicial, amplitudMaxima)
                );
            plnEjeX.Points.Add(
                adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial,
                amplitudMaxima)
                );
            //Resultado
            plnEjeXResultante.Points.Clear();
            plnEjeXResultante.Points.Add((
                adaptarCoordenadas(tiempoInicial, 0.0,
                tiempoInicial, amplitudMaxima)));
            plnEjeXResultante.Points.Add(
                adaptarCoordenadas(tiempoFinal, 0.0,
                tiempoInicial, amplitudMaxima));
            //ver el cambio de como afecto la escala de la grafica
            
            //Original
            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(
                adaptarCoordenadas(0.0, amplitudMaxima,
                tiempoInicial,amplitudMaxima));
            plnEjeY.Points.Add(
                adaptarCoordenadas(0.0, amplitudMaxima * -1,
                tiempoInicial, amplitudMaxima)) ;

            //Resultado
            plnEjeYResultante.Points.Clear();
            plnEjeYResultante.Points.Add(
                adaptarCoordenadas(0.0,amplitudMaxima,
                tiempoInicial, amplitudMaxima)
                );
            plnEjeYResultante.Points.Add(
                adaptarCoordenadas(0.0, amplitudMaxima * -1,
                tiempoInicial, amplitudMaxima)
                );


        }

        public Point adaptarCoordenadas(
            double x, double y, double tiempoInicial,
            double amplitudMaxima)
        {
            return new Point((x - tiempoInicial) * scrGrafica.Width,
                (-1 * (
                y * ((( scrGrafica.Height  / 2.0  ) -25 ) / amplitudMaxima) )) +
                (scrGrafica.Height / 2.0) );
        }

        private void CbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch(cbTipoSeñal.SelectedIndex)
            {
                case 0: //Parabolica
                    break;
                case 1: //Senoidal
                    panelConfiguracion.Children.Add(
                        new ConfiguracionSeñalSenoidal());
                    break;
                case 2:
                    panelConfiguracion.Children.Add(
                        new ConfiguracionAudio()
                        ) ;
                    break;
                default:
                    break;
            }
        }

        private void CbOperacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracionOperacion.Children.Clear();
            mostrarSegundaSeñal(false);
            switch(cbOperacion.SelectedIndex)
            {
                case 0: //Escala de amplitud
                    panelConfiguracionOperacion.
                        Children.Add(
                            new OperacionEscalaAmplitud()
                        );
                    break;
                case 1: //Desplazamiento de Amplitud
                    panelConfiguracionOperacion.Children.Add(new DesplazamientoAmplitud());
                    break;

                case 2:
                    mostrarSegundaSeñal(true);
                    //panelConfiguracionOperacion.Children.Add((new));
                    break;
                case 3:
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaExponencial());
                    break;
                case 4:
                    mostrarSegundaSeñal(true);
                    break;
                
                default:
                    break;
            }
        }
       
        private void CbTipoSeñal_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion_2.Children.Clear();
            switch (cbTipoSeñal_2.SelectedIndex)
            {
                case 0: //Parabolica
                    break;
                case 1: //Senoidal
                    panelConfiguracion_2.Children.Add(
                        new ConfiguracionSeñalSenoidal());
                    break;
                case 2:
                    panelConfiguracion_2.Children.Add(
                        new ConfiguracionAudio()
                        );
                    break;
                default:
                    break;
            }

        }

        void mostrarSegundaSeñal(bool mostrar)
        {
            if (mostrar)
            {
                lblTipoSeñal_2.Visibility = Visibility.Visible;
                cbTipoSeñal_2.Visibility = Visibility.Visible;
                panelConfiguracion_2.Visibility = Visibility.Visible;
            }
            else
            {
                lblTipoSeñal_2.Visibility = Visibility.Hidden;
                cbTipoSeñal_2.Visibility = Visibility.Hidden;
                panelConfiguracion_2.Visibility = Visibility.Hidden;
            }
        }
    }
}
