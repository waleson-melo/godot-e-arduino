using Godot;
using System.IO.Ports;

public partial class Main : Node2D
{
	SerialPort serialPort;
	string buffer = "";

	AnimatedSprite2D sprite;
	RichTextLabel locked;

	public override void _Ready()
	{
		serialPort = new SerialPort();
		serialPort.PortName = "COM6";
		serialPort.BaudRate = 9600;
		serialPort.Open();

		sprite = GetNode<AnimatedSprite2D>("Sprite");
		locked = GetNode<RichTextLabel>("Locked");
	}

	public override void _Process(double delta)
	{
		if (!serialPort.IsOpen)
			return;

		buffer += serialPort.ReadExisting();

		// Processa mensagens completas com delimitador ';'
		while (buffer.Contains(";"))
		{
			int delimiterIndex = buffer.IndexOf(';');
			string message = buffer.Substring(0, delimiterIndex).Trim();
			buffer = buffer.Substring(delimiterIndex + 1); // Remove a mensagem processada

			// BTN1-L; BTN1-U; BTN1-B;	

			// GD.Print(message);

			char id = message[3];
			char action = message[5];

			locked.Text = $"Processo {id}";

			switch (action)
			{
				case 'L':
					sprite.Animation = "red";
					break;
				case 'U':
					sprite.Animation = "green";
					break;
				case 'B':
					sprite.Animation = "yellow";
					break;
			}
		}
	}
}
