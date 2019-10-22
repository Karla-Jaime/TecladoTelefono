using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GraficadorSeñales
{
    abstract class Señal
    {
        public List<Muestra> Muestras { get; set; }
        public double TiempoInicial { get; set; }
        public double TiempoFinal { get; set; }
        public double FrecuenciaMuestreo { get; set; }

        public double AmplitudMaxima { get; set; }

        public abstract double evaluar(double tiempo);

        public void construirSeñal()
        {
            double periodoMuestreo =
                1 / FrecuenciaMuestreo;

            Muestras.Clear();

            for(double i = TiempoInicial;
                i <= TiempoFinal;
                i += periodoMuestreo)
            {
                double muestra = evaluar(i);

                Muestras.Add(
                    new Muestra(i, muestra));

                if (Math.Abs(muestra) >
                    AmplitudMaxima)
                {
                    AmplitudMaxima =
                        Math.Abs(muestra);
                }

            }
        }

        public static Señal escalarAmplitud(
            Señal señalOriginal, double factorEscala)
        {

            SeñalResultante resultado =
                new SeñalResultante();
            resultado.TiempoInicial = señalOriginal.TiempoInicial;
            resultado.TiempoFinal = señalOriginal.TiempoFinal;
            resultado.FrecuenciaMuestreo =
                señalOriginal.FrecuenciaMuestreo;

            foreach(var muestra in señalOriginal.Muestras)
            { 
                
                double nuevoValor = muestra.Y * factorEscala;
                resultado.Muestras.Add(
                    new Muestra(muestra.X,  nuevoValor));
                if (Math.Abs(nuevoValor) > resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima =
                        Math.Abs(nuevoValor);
                }
            }

            return resultado;
        }
        public static Señal transformadaFourier(Señal señal)
        {
            SeñalResultante resultado = new SeñalResultante();
            resultado.TiempoInicial = señal.TiempoInicial;
            resultado.TiempoFinal = señal.TiempoFinal;
            resultado.FrecuenciaMuestreo = señal.FrecuenciaMuestreo;
            //Integral 

            for (int k = 0; k < señal.Muestras.Count; k++){
                //Xk
                Complex muestra = 0;// 0 + 01
                for (int n = 0; n < señal.Muestras.Count; n++)
                {//Sumatoria n= 0 + acumulados
                    muestra += //Xn       * (e^-2Pi*i*k*n)/N
                        señal.Muestras[n].Y * Complex.Exp((-2 * Math.PI * Complex.ImaginaryOne * k * n) / señal.Muestras.Count);
                }
                //Para graficar num complejos se camia el valor complejo a valor absoluto o magnitud
                resultado.Muestras.Add(new Muestra( señal.Muestras[k].X, muestra.Magnitude));
                //Estoy bien mareada :( </3
                if (Math.Abs(muestra.Magnitude) > señal.AmplitudMaxima)
                {
                    señal.AmplitudMaxima = Math.Abs(muestra.Magnitude);
                }
                // ( i * fm)/ N (num de muestras)indice de Fourier a Hz
                
                          
            }
            return resultado;
        }

        public static Señal multiplicarSeñales(Señal señal1, Señal señal2)
        {
            //
            SeñalResultante resultado = new SeñalResultante();
            resultado.TiempoInicial = señal1.TiempoInicial;
            resultado.TiempoFinal = señal1.TiempoInicial;
            resultado.FrecuenciaMuestreo = señal1.FrecuenciaMuestreo;

            int indice = 0;
             foreach (var muestra in señal1.Muestras)
            {//
                double nuevoValor = muestra.Y * señal2.Muestras[indice].Y;

                resultado.Muestras.Add(new Muestra(muestra.X, nuevoValor));
                if (Math.Abs(nuevoValor)> resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima = Math.Abs(nuevoValor);
                }
                indice++;
            }
            return resultado;
        }

        public static Señal adicionarSeñales(Señal señal1, Señal señal2)
        {
            //
            SeñalResultante resultado = new SeñalResultante();
            resultado.TiempoInicial = señal1.TiempoInicial;
            resultado.TiempoFinal = señal1.TiempoInicial;
            resultado.FrecuenciaMuestreo = señal1.FrecuenciaMuestreo;

            int indice = 0;
            foreach (var muestra in señal1.Muestras)
            {//
                double nuevoValor = muestra.Y + señal2.Muestras[indice].Y;

                resultado.Muestras.Add(new Muestra(muestra.X, nuevoValor));
                if (Math.Abs(nuevoValor) > resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima = Math.Abs(nuevoValor);
                }
                indice++;
            }
            return resultado;
        }

        public static Señal desAmplitud(Señal señalOriginal, double factorDesplazamiento)
        {
            SeñalResultante resultado = new SeñalResultante();

            resultado.TiempoInicial = señalOriginal.TiempoInicial;
            resultado.TiempoFinal = señalOriginal.TiempoFinal;
            resultado.FrecuenciaMuestreo =
                señalOriginal.FrecuenciaMuestreo;
            foreach (var muestra in señalOriginal.Muestras)
            {
                double nuevoValor = muestra.Y + factorDesplazamiento;
                resultado.Muestras.Add( new Muestra( muestra.X,nuevoValor)
                    );
                if (Math.Abs(nuevoValor) > resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima =
                        Math.Abs(nuevoValor);
                }
            }
            return resultado;
        }

        public static Señal escalaExponencial(Señal señaOriginal, double factorExponencial)
        {
            SeñalResultante resultado = new SeñalResultante();
            resultado.TiempoInicial = señaOriginal.TiempoInicial;
            resultado.TiempoFinal = señaOriginal.TiempoFinal;
            resultado.FrecuenciaMuestreo = señaOriginal.FrecuenciaMuestreo;

            foreach (var muestra in señaOriginal.Muestras)
            {
               
                double nuevoValor = Math.Pow(muestra.Y, factorExponencial);
                resultado.Muestras.Add(new Muestra(muestra.X, nuevoValor));
                if (Math.Abs(nuevoValor) > resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima = Math.Abs(nuevoValor);
                }
            }
            return resultado;
        }
    }

}
