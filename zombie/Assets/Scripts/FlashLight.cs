using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashLight : MonoBehaviour
{

	public KeyCode control = KeyCode.F; // клавиша управления фонариком, вкл/выкл и перезарядка
	public float timeout = 30; // время работы фонарика в секундах

	public Light _light; // источник света

	// минимальная и максимальная интенсивность источника света
	public float min = 1;
	public float max = 8;

	public Slider slider; // элемент UI

	// цвета индикатора
	public Color maxColor = Color.white;
	public Color halfColor = Color.yellow;
	public Color minColor = Color.red;

	public float reloadTime = 2.5f; // время перезарядки в секундах
	public int batteryCount = 3; // доступное количество запасных батареек

	private float curTime;
	private Image sliderColor;
	private float curReloadTime;

	void Awake()
	{
		sliderColor = slider.fillRect.GetComponentInChildren<Image>();
	}

	void Start()
	{
		sliderColor.color = maxColor;
		slider.minValue = 0;
		slider.maxValue = 100;
		slider.value = 100;
		_light.enabled = false;
		_light.intensity = min;
	}

	void Update()
	{
		if (Input.GetKeyDown(control) && !_light.enabled && slider.value > 1)
		{
			_light.enabled = true;
		}
		else if (Input.GetKeyDown(control) && _light.enabled)
		{
			_light.enabled = false;
			_light.intensity = min;
		}

		if (_light.enabled)
		{
			curTime += Time.deltaTime;

			if (curTime > timeout)
			{
				curTime = 0;
				slider.value = 0;
				_light.enabled = false;
			}
			else if (curTime != 0)
			{
				// переводим время в диапазон значений от 0 до 100% и вычетам из 100
				slider.value = 100 - (curTime / timeout) * 100;
			}

			float intensity = max;
			Color curColor = maxColor;

			if (slider.value < 50) curColor = halfColor; // меняем цвет на промежуточный, если меньше 50% заряда

			if (slider.value < 20)
			{
				curColor = minColor;
				intensity = max / 2; // снижаем яркость фонарика

				if (Random.Range(0, 0.9f) > 0.5f) intensity = intensity / Random.Range(1, 6); // рандомное мерцание, перед отключением
			}

			sliderColor.color = Color.Lerp(sliderColor.color, curColor, 1.5f * Time.deltaTime);
			_light.intensity = Mathf.Lerp(_light.intensity, intensity, 3f * Time.deltaTime);
		}
		else if (Input.GetKey(control) && slider.value < 90 && batteryCount > 0) // перезарядка, если заряда батареи меньше 90% и есть еще запасные
		{
			curReloadTime += Time.deltaTime;
			if (curReloadTime > reloadTime)
			{
				curReloadTime = 0;
				batteryCount--;
				sliderColor.color = maxColor;
				slider.value = 100;
				_light.intensity = min;
			}
		}
		else
		{
			curReloadTime = 0;
		}
	}
}