using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using UnityEngine;

namespace Chatterer
{
    public partial class chatterer : MonoBehaviour
    {
        IEnumerator Exchange(float delay)
        {
            if (debugging) Debug.Log("[CHATR] Beginning Cor_exchange");

            exchange_playing = true;
            initialize_new_exchange();

            if (initial_chatter_source == 1)
            {
                //capsule starts the exchange
                //always play regardless of radio contact state
                if (debugging) Debug.Log("[CHATR] Capsule starts the exchange...");

                //play initial capsule chatter
                if (initial_chatter_set.Count > 0)
                {
                    if (initial_chatter.isPlaying == false)
                    {
                        initial_chatter.Play();
                        if (debugging) Debug.Log("[CHATR] playing initial chatter");
                        yield return new WaitForSeconds(initial_chatter.clip.length);
                        //initial chatter has finished playing
                    }
                    else if (debugging) Debug.LogWarning("[CHATR] initial_chatter already playing, move on...");
                }
                else
                {
                    exchange_playing = false;
                    if (debugging) Debug.LogWarning("[CHATR] initial_chatter_set has no audioclips, abandoning exchange");
                }
            }
            else if (initial_chatter_source == 0)
            {
                //capcom starts the exchange
                if (debugging) Debug.Log("[CHATR] Capcom starts the exchange...");

                if (inRadioContact)
                {
                    //in radio contact,
                    //play initial capcom

                    if (initial_chatter_set.Count > 0)
                    {
                        if (quindar_toggle)
                        {
                            initial_chatter.PlayOneShot(quindar1.clip); // Workaround [playing quindar with quindar.audiosource is BROKEN : hangs exchange since KSP v1.4.0 for some weird reasons]
                            if (debugging) Debug.Log("[CHATR] playingOneShot initial quindar");
                            yield return new WaitForSeconds(quindar1.clip.length);

                            initial_chatter.Play();
                            if (debugging) Debug.Log("[CHATR] playing initial chatter");
                            yield return new WaitForSeconds(initial_chatter.clip.length);

                            initial_chatter.PlayOneShot(quindar2.clip); // Workaround [playing quindar with quindar.audiosource is BROKEN : hangs exchange since KSP v1.4.0 for some weird reasons]
                            if (debugging) Debug.Log("[CHATR] playingOneShot initial quindar");
                            yield return new WaitForSeconds(quindar2.clip.length);

                            //initial chatter has finished playing

                        }
                        else                  // play without quindar
                        {
                            initial_chatter.Play();
                            if (debugging) Debug.Log("[CHATR] playing initial chatter");
                            yield return new WaitForSeconds(initial_chatter.clip.length);
                            //initial chatter has finished playing
                        }
                    }
                    else
                    {
                        exchange_playing = false;
                        if (debugging) Debug.LogWarning("[CHATR] initial_chatter_set has no audioclips, abandoning exchange");
                    }
                }
                else
                {
                    //not in radio contact,
                    //play no initial chatter or response
                    if (debugging) Debug.Log("[CHATR] No radio contact, Capcom is speaking with void.");
                    exchange_playing = false;
                }
            }
            
            //so respond now
            if (debugging) Debug.Log("[CHATR] Responding Cor_exchange, delay = " + response_delay_secs);

            yield return new WaitForSeconds(response_delay_secs);

            if (response_chatter_set.Count > 0 && (inRadioContact))
            {
                if (debugging) Debug.Log("[CHATR] playing response");
                if (initial_chatter_source == 1)
                {
                    if (debugging) Debug.Log("[CHATR] Capcom responding");

                    if (quindar_toggle)
                    {
                        response_chatter.PlayOneShot(quindar1.clip); // Workaround [playing quindar with quindar.audiosource is BROKEN : hangs exchange since KSP v1.4.0 for some weird reasons]
                        if (debugging) Debug.Log("[CHATR] playingOneShot response quindar");
                        yield return new WaitForSeconds(quindar1.clip.length);

                        response_chatter.Play();
                        if (debugging) Debug.Log("[CHATR] playing response chatter");
                        yield return new WaitForSeconds(response_chatter.clip.length);

                        response_chatter.PlayOneShot(quindar2.clip); // Workaround [playing quindar with quindar.audiosource is BROKEN : hangs exchange since KSP v1.4.0 for some weird reasons]
                        if (debugging) Debug.Log("[CHATR] playingOneShot response quindar");
                        yield return new WaitForSeconds(quindar2.clip.length);

                        //response chatter has finished playing

                    }
                    else                  // play without quindar
                    {
                        response_chatter.Play();
                        if (debugging) Debug.Log("[CHATR] playing response chatter");
                        yield return new WaitForSeconds(response_chatter.clip.length);
                        //response chatter has finished playing
                    }
                }
                else if (initial_chatter_source == 0)
                {
                    if (response_chatter.isPlaying == false)
                    {
                        if (debugging) Debug.Log("[CHATR] Capsule responding");

                        response_chatter.PlayDelayed(delay);

                        yield return new WaitForSeconds(response_chatter.clip.length);
                        //response chatter has finished playing
                    }
                    else if (debugging) Debug.LogWarning("[CHATR] response_chatter already playing, move on...");
                }
            }
            else if (response_chatter_set.Count > 0 && !inRadioContact)
            {
                if (exchange_playing == true)
                {
                    if (debugging) Debug.Log("[CHATR] No connection, no response ... you are alone !");
                    exchange_playing = false;
                }
            }
            else
            {
                if (debugging) Debug.LogWarning("[CHATR] response_chatter_set has no audioclips, abandoning exchange");
                exchange_playing = false;   //exchange is over
            }

            exchange_playing = false;
            if (debugging) Debug.Log("[CHATR] exchange is over");
        }
    }
}
