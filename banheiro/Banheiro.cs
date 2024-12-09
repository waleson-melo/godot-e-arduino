using Godot;
using System.IO.Ports;

public partial class Banheiro : Node2D
{
	SerialPort serialPort;
	string buffer = "";

	RichTextLabel info;
	AnimationPlayer animation;
	char lastBanheiro = ' ';
	AnimatedSprite2D personagem1;
	AnimatedSprite2D personagem2;

	public override void _Ready()
	{
		serialPort = new SerialPort();
		serialPort.PortName = "COM6";
		serialPort.BaudRate = 9600;
		serialPort.Open();

		info = GetNode<RichTextLabel>("Center/Info");
		animation = GetNode<AnimationPlayer>("Animation");
		personagem1 = GetNode<AnimatedSprite2D>("Personagem1/Sprite");
		personagem2 = GetNode<AnimatedSprite2D>("Personagem2/Sprite");
	}

	public override void _Process(double delta)
	{
		if (!serialPort.IsOpen)
			return;

		buffer += serialPort.ReadExisting();

		while (buffer.Contains(";"))
		{
			int delimiterIndex = buffer.IndexOf(';');
			string message = buffer.Substring(0, delimiterIndex).Trim();
			buffer = buffer.Substring(delimiterIndex + 1);

			char id = message[3];
			char action = message[5];

			switch (action)
			{
				case 'L':
					if (id == '1')
					{
						animation.Play("p1-entrar");
						personagem1.Play("run");
						lastBanheiro = id;
					}
					else
					{
						animation.Play("p2-entrar");
						personagem2.Play("run");
						lastBanheiro = id;
					}
					info.Text = "Ocupado";
					break;
				case 'U':
					if (lastBanheiro == '1')
					{
						animation.Play("p1-sair");
						personagem1.Play("run");
					}
					else
					{
						animation.Play("p2-sair");
						personagem2.Play("run");
					}
					info.Text = "Livre";
					break;
				case 'B':
					if (id == '1')
						animation.Play("p1-apertado");
					else
						animation.Play("p2-apertado");
					break;
			}
		}
	}
}
